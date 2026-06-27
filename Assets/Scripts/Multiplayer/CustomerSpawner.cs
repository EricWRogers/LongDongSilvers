using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class CustomerSpawner : NetworkBehaviour
{
    public static CustomerSpawner Instance;

    public GameObject customerPrefab;
    public Transform spawnPoint;
    public Transform exitPoint;
    public float spawnInterval = 10f;

    void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        StartCoroutine(WaitForGameManager());
    }

    IEnumerator WaitForGameManager()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.shiftStarted.OnValueChanged += OnShiftStarted;
        Debug.Log("CustomerSpawner hooked into GameManager");
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.shiftStarted.OnValueChanged -= OnShiftStarted;
    }

    void OnShiftStarted(bool previous, bool current)
    {
        if (current && IsHost)
            StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (GameManager.Instance.shiftStarted.Value)
        {
            SpawnCustomer();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCustomer()
    {
        var go = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);
        go.GetComponent<NetworkObject>().Spawn();
    }
}
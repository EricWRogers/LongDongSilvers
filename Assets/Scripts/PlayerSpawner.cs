using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        if (!NetworkManager.Singleton.IsHost) return;

        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            GameObject player = Instantiate(playerPrefab);

            player.GetComponent<NetworkObject>()
                  .SpawnAsPlayerObject(client.ClientId);
        }
    }
}
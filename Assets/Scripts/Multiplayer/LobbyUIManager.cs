using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
   public Transform container;
    public GameObject entryPrefab;

    private readonly List<GameObject> players = new();

    public TMP_Text joinCodeText;

    void OnEnable()
    {
        //Very handy for watching players join n leave.
        NetworkManager.Singleton.OnClientConnectedCallback += Refresh;
        NetworkManager.Singleton.OnClientDisconnectCallback += Refresh;
    }

    void OnDisable()
    {
        if (NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.OnClientConnectedCallback -= Refresh;
        NetworkManager.Singleton.OnClientDisconnectCallback -= Refresh;
    }

    void Start()
    {
        joinCodeText.text = GameManager.Instance.JoinCode;
        
        //Rebuild on start.
        Refresh(0);
    }

    void Refresh(ulong _) //It is what it is. Netcode wants a ulong.
    {
        Rebuild();
    }

    void Rebuild()
    {
        for (int i = 0; i < players.Count; i++)
            Destroy(players[i]);

        players.Clear();

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var obj = Instantiate(entryPrefab, container);
            obj.GetComponent<PlayerLobbyUI>()
               .SetName($"Player {client.ClientId}");

            players.Add(obj);
        }
    }

}

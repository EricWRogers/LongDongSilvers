using UnityEngine;
using Unity.Netcode;

public class OrderManager : NetworkBehaviour
{
    public static OrderManager Instance;

    private ulong[] slotIds = new ulong[3] { 0, 0, 0 };
    private string[] slotIngredients = new string[3] { "", "", "" };

    public event System.Action OrdersUpdated;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void AddOrder(ulong customerId, string ingredientNames)
    {
        if (!IsServer) return;
        AddOrderClientRpc(customerId, ingredientNames);
    }

    [ClientRpc]
    void AddOrderClientRpc(ulong customerId, string ingredientNames)
    {
        for (int i = 0; i < 3; i++)
        {
            if (slotIds[i] == 0)
            {
                slotIds[i] = customerId;
                slotIngredients[i] = ingredientNames;
                OrdersUpdated?.Invoke();
                return;
            }
        }
    }

    public void ClearOrder(ulong customerId)
    {
        if (!IsServer) return;
        ClearOrderClientRpc(customerId);
    }

    [ClientRpc]
    void ClearOrderClientRpc(ulong customerId)
    {
        for (int i = 0; i < 3; i++)
        {
            if (slotIds[i] == customerId)
            {
                slotIds[i] = 0;
                slotIngredients[i] = "";
                OrdersUpdated?.Invoke();
                return;
            }
        }
    }

    public ulong GetSlotId(int slot) => slotIds[slot];
    public string GetSlotIngredients(int slot) => slotIngredients[slot];
}
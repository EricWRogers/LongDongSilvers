using Unity.Netcode;
using UnityEngine;


public class Item : NetworkBehaviour
{
    [Header("Item Info")]
    public string itemName = "Item";
    public ItemType itemType = ItemType.Food;

    public enum ItemType { Food, Utensil, Ingredient }

    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }


    public void PickUp(Transform holdPoint)
    {
        if (!IsServer) return;

        rb.isKinematic = true;
        col.enabled = false;

        if (!NetworkObject.TrySetParent(holdPoint, worldPositionStays: false))
            Debug.LogWarning($"[Item] TrySetParent failed for {itemName}.");

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop(Vector3 dropPosition)
    {
        if (!IsServer) return;

        // Unparent first, then set world position.
        NetworkObject.TrySetParent((Transform)null, worldPositionStays: false);

        transform.position = dropPosition;
        rb.isKinematic = false;
        col.enabled = true;
    }
}
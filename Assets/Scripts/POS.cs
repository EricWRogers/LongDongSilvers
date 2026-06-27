using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class POS : NetworkBehaviour
{
    public GameObject startShift;
    public GameObject ingredientSelect;

    public TextMeshProUGUI text;
    
    public List<FoodIngredientDefinition> ingredientsForOrder = new List<FoodIngredientDefinition>();
    
    public string orderText;
    
    public override void OnNetworkSpawn()
    {
        UpdatePanelClientRpc(true, false);
    }
    
    public void StartShift()
    {
        StartShiftServerRpc();
    }

    public void SubmitOrder()
    {
        RegisterTest.Instance.NotifyOrderSubmittedServerRpc();
        GameManager.Instance.SubmitOrderServerRpc(RegisterTest.Instance.CurrentCustomerId);
        ingredientsForOrder.Clear();
        orderText = "";
        SubmitOrderServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void StartShiftServerRpc()
    {
        GameManager.Instance.StartShiftServerRpc();
        UpdatePanelClientRpc(false, false);
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitOrderServerRpc()
    {
        UpdatePanelClientRpc(false, false);
    }

    [ClientRpc]
    void UpdatePanelClientRpc(bool showStart, bool showIngredient)
    {
        startShift.SetActive(showStart);
        ingredientSelect.SetActive(showIngredient);
    }

    public void AddIngredient(FoodIngredientButtonDefinition ingredient)
    {
        ingredientsForOrder.Add(ingredient.ingredient);
        orderText += ingredient.ingredient.IngredientName + "\n";
        text.text = orderText;
    }
}

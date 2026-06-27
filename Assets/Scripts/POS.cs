using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class POS : NetworkBehaviour
{
    public GameObject startShift;
    public GameObject mealSelect;
    public GameObject ingredientSelect;

    public TextMeshProUGUI text;

    public FoodIngredientDefinition dong;
    public FoodIngredientDefinition patty;
    
    private bool isThisPatty = false;

    public List<FoodIngredientDefinition> ingredientsForOrder = new List<FoodIngredientDefinition>();
    
    public string orderText;
    
    public override void OnNetworkSpawn()
    {
        UpdatePanelClientRpc(true, false, false);
    }
    public void StartShift()
    {
        StartShiftServerRpc();
    }

    public void SelectBurger()
    {
        ingredientsForOrder.Add(patty);
        isThisPatty = true;
        orderText = "";
        SelectBurgerServerRpc();
    }

    public void SelectHotdog()
    {
        ingredientsForOrder.Add(dong);
        isThisPatty = false;
        orderText = "";
        SelectHotdogServerRpc();
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
        UpdatePanelClientRpc(false, true, false);
    }

    [ServerRpc(RequireOwnership = false)]
    void SelectBurgerServerRpc()
    {
        UpdatePanelClientRpc(false, false, true);
    }

    [ServerRpc(RequireOwnership = false)]
    void SelectHotdogServerRpc()
    {
        UpdatePanelClientRpc(false, false, true);
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitOrderServerRpc()
    {
        UpdatePanelClientRpc(false, true, false);
    }

    [ClientRpc]
    void UpdatePanelClientRpc(bool showStart, bool showMeal, bool showIngredient)
    {
        startShift.SetActive(showStart);
        mealSelect.SetActive(showMeal);
        ingredientSelect.SetActive(showIngredient);
    }
}

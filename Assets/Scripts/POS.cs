using System;
using Unity.Netcode;
using UnityEngine;

public class POS : NetworkBehaviour
{
    public GameObject startShift;
    public GameObject mealSelect;
    public GameObject ingredientSelect;

    public void Start()
    {
        startShift.SetActive(true);
        mealSelect.SetActive(false);
        ingredientSelect.SetActive(false);
    }

    public void StartShift()
    {
        startShift.SetActive(false);
        mealSelect.SetActive(true);
    }

    public void SelectBurger()
    {
        mealSelect.SetActive(false);
        ingredientSelect.SetActive(true);
        // Add burger to list
    }
    
    public void SelectHotdog()
    {
        mealSelect.SetActive(false);
        ingredientSelect.SetActive(true);
        // Add hotdog to list
    }

    public void SubmitOrder()
    {
        ingredientSelect.SetActive(false);
        mealSelect.SetActive(true);
    }
}

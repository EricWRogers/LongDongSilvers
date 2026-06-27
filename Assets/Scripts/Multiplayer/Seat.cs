using UnityEngine;

public class Seat : MonoBehaviour
{
    public bool IsOccupied;

    public void Claim()
    {
        IsOccupied = true;
    } 
    public void Vacate() 
    {
        IsOccupied = false;
    } 
}
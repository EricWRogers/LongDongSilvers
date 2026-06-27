public enum OrderState
{
    Submitted,
    Ready,
    Delivered,
}

public struct Order
{
    public ulong CustomerId;
    public OrderState State;
    public string[] WantedIngredients;
    public string[] ReceivedIngredients;
}
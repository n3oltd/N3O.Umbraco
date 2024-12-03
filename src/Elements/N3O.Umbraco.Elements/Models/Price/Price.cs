namespace N3O.Umbraco.Elements.Models;

public class Price : Value, IPrice {
    public Price(decimal amount, bool locked) {
        Amount = amount;
        Locked = locked;
    }

    public decimal Amount { get; }
    public bool Locked { get; }
}

using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class Price : Value, IPrice {
    [JsonConstructor]
    public Price(decimal amount, bool locked) {
        Amount = amount;
        Locked = locked;
    }

    public Price(IPrice price) : this(price.Amount, price.Locked) { }

    public decimal Amount { get; }
    public bool Locked { get; }
}

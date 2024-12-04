namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IPrice {
    decimal Amount { get; }
    bool Locked { get; }
}

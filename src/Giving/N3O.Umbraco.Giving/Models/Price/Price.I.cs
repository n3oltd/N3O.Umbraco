namespace N3O.Umbraco.Giving.Models;

public interface IPrice {
    decimal Amount { get; }
    bool Locked { get; }
}

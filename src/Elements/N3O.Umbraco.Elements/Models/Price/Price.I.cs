namespace N3O.Umbraco.Elements.Models;

public interface IPrice {
    decimal Amount { get; }
    bool Locked { get; }
}

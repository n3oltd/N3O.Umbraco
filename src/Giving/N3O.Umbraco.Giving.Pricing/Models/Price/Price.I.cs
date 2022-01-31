namespace N3O.Umbraco.Giving.Pricing.Models {
    public interface IPrice {
        decimal Amount { get; }
        bool Locked { get; }
    }
}
namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IPricing : IPrice {
    IEnumerable<IPricingRule> Rules { get; }
}

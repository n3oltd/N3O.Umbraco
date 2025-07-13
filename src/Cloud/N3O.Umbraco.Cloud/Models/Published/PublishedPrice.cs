using N3O.Umbraco.Giving.Allocations.Models;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedPrice : IPrice {
    public decimal Amount { get; set; }
    public bool Locked { get; set; }
}

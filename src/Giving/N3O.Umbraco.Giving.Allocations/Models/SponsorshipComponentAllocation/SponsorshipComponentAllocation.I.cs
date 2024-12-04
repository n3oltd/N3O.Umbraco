using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models;

public interface ISponsorshipComponentAllocation {
    SponsorshipComponent Component { get; }
    Money Value { get; }
}

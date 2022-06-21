using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public interface ISponsorshipComponentAllocation {
    SponsorshipComponent Component { get; }
    Money Value { get; }
}

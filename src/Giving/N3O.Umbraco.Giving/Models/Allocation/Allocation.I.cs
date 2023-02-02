using N3O.Umbraco.Financial;
using N3O.Giving.Models;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public interface IAllocation {
    AllocationType Type { get; }
    Money Value { get; }
    IFundDimensionValues FundDimensions { get; }
    IFundAllocation Fund { get; }
    ISponsorshipAllocation Sponsorship { get; }
    bool Upsell { get; }
}

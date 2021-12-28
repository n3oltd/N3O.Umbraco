using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IAllocation {
    AllocationType Type { get; }
    Money Value { get; }
    FundDimension1Option Dimension1 { get; }
    FundDimension2Option Dimension2 { get; }
    FundDimension3Option Dimension3 { get; }
    FundDimension4Option Dimension4 { get; }
    IFundAllocation Fund { get; }
    ISponsorshipAllocation Sponsorship { get; }
}
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;

namespace N3O.Umbraco.Cloud.Platforms.Content;

public interface IHoldDonationFormState {
    DonationItem DonationItem { get; }
    FundDimension1Value Dimension1 { get; }
    FundDimension2Value Dimension2 { get; }
    FundDimension3Value Dimension3 { get; }
    FundDimension4Value Dimension4 { get; }
}
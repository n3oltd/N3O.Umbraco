using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public class PriceCriteria {
    [Name("Donation Item")]
    public DonationItem DonationItem { get; set; }
    
    [Name("Sponsorship Component")]
    public SponsorshipComponent SponsorshipComponent { get; set; }

    [Name("Fund Dimensions")]
    public FundDimensionValuesReq FundDimensions { get; set; }
}

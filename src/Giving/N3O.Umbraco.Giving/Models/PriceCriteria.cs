using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Models;

public class PriceCriteria {
    [Name("Donation Item")]
    public DonationItem DonationItem { get; set; }
    
    [Name("Feedback Scheme")]
    public FeedbackScheme FeedbackScheme { get; set; }
    
    [Name("Sponsorship Component")]
    public SponsorshipComponent SponsorshipComponent { get; set; }

    [NoValidation]
    [Name("Fund Dimensions")]
    public FundDimensionValuesReq FundDimensions { get; set; }
}

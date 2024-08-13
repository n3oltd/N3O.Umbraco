using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.CrowdFunding;

public class FundraiserTitleValidator : ContentPropertyValidator<TextBoxValueReq> {
    private const int MaxLength = 100;
    
    public FundraiserTitleValidator()
        : base(CrowdfundingConstants.Fundraiser.Alias, CrowdfundingConstants.Fundraiser.Properties.Title) { }
    
    // TODO Talha this should really take in a formatter also and return a validaiton failure or null rather than
    // just true/false so we can indicate why it failed
    protected override bool IsValid(IPublishedContent content, string propertyAlias, TextBoxValueReq req) {
        if (req.Value.Length > MaxLength) {
            return false;
        }

        return true;
    }
}
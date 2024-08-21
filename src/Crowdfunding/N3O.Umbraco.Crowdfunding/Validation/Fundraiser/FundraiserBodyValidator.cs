using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.CrowdFunding;

public class FundraiserBodyValidator : ContentPropertyValidator<RawValueReq, RawConfigurationRes> {
    private const int MaxLength = 1000;
    
    public FundraiserBodyValidator(IFormatter formatter)
        : base(formatter, CrowdfundingConstants.Fundraiser.Alias, CrowdfundingConstants.Fundraiser.Properties.Body) { }
    
    protected override void Validate(IPublishedContent content, string propertyAlias, RawValueReq req) {
        if (req.Value.Length > MaxLength) {
            AddFailure<Strings>(propertyAlias, x => x.MaxLength, MaxLength);
        }
    }
    
    protected override void PopulatePropertyConfiguration(IPropertyType property, RawConfigurationRes res) {
        res.MaximumLength = MaxLength;
        res.Description = property.Description;
    }  
    
    public class Strings : ValidationStrings {
        public string MaxLength => $"Body cannot exceed {"{0}".Quote()} characters.";
    }
}
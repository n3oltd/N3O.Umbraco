using N3O.Umbraco.Data;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding;

public class FundraiserNameValidator : ContentPropertyValidator<TextBoxValueReq, TextBoxConfigurationRes> {
    private const int MaxLength = 100;
    
    public FundraiserNameValidator(IFormatter formatter)
        : base(formatter, CrowdfundingConstants.Fundraiser.Alias, CrowdfundingConstants.Crowdfunder.Properties.Name) { }
    
    protected override void PopulatePropertyConfiguration(IPropertyType property, TextBoxConfigurationRes res) {
        res.MaximumLength = MaxLength;
        res.Description = property.Description;
    }
    
    protected override void Validate(IPublishedContent content, string propertyAlias, TextBoxValueReq req) {
        if (req.Value.Length > MaxLength) {
            AddFailure<Strings>(propertyAlias, x => x.MaxLength, MaxLength);
        }
    }
    
    public class Strings : ValidationStrings {
        public string MaxLength => $"Name cannot exceed {"{0}".Quote()} characters.";
    }
}
using N3O.Umbraco.Data;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding;

public class FundraiserNameValidator : ContentPropertyValidator<TextBoxValueReq, TextBoxConfigurationRes> {
    public FundraiserNameValidator(IFormatter formatter)
        : base(formatter, CrowdfundingConstants.Fundraiser.Alias, CrowdfundingConstants.Crowdfunder.Properties.Name) { }
    
    protected override void PopulatePropertyConfiguration(IPropertyType property, TextBoxConfigurationRes res) {
        res.MaximumLength = CrowdfundingConstants.Crowdfunder.NameMaxLength;
        res.Description = property.Description;
    }
    
    protected override void Validate(IPublishedContent content, string propertyAlias, TextBoxValueReq req) {
        if (req.Value.Length > CrowdfundingConstants.Crowdfunder.NameMaxLength) {
            AddFailure<Strings>(propertyAlias, x => x.MaxLength, CrowdfundingConstants.Crowdfunder.NameMaxLength);
        }
    }
    
    public class Strings : ValidationStrings {
        public string MaxLength => $"Name cannot exceed {"{0}".Quote()} characters.";
    }
}
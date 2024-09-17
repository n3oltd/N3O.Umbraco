using N3O.Umbraco.Data;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding;

public class FundraiserDescriptionValidator : ContentPropertyValidator<TextareaValueReq, TextareaConfigurationRes> {
    private const int MaxLength = 200;
    
    public FundraiserDescriptionValidator(IFormatter formatter)
        : base(formatter, CrowdfundingConstants.Fundraiser.Alias, CrowdfundingConstants.Crowdfunder.Properties.Description) { }
    
    protected override void Validate(IPublishedContent content, string propertyAlias, TextareaValueReq req) {
        if (req.Value.Length > MaxLength) {
            AddFailure<Strings>(propertyAlias, x => x.MaxLength, MaxLength);
        }
    }
    
    protected override void PopulatePropertyConfiguration(IPropertyType property, TextareaConfigurationRes res) {
        res.MaximumLength = MaxLength;
        res.Description = property.Description;
    }
    
    public class Strings : ValidationStrings {
        public string MaxLength => $"Description cannot exceed {"{0}".Quote()} characters.";
    }
}
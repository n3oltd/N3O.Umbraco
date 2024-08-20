using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.CrowdFunding;

public class FundraiserTitleValidator : ContentPropertyValidator<TextBoxValueReq> {
    private const int MaxLength = 100;
    
    public FundraiserTitleValidator(IFormatter formatter)
        : base(formatter, CrowdfundingConstants.Fundraiser.Alias, CrowdfundingConstants.Fundraiser.Properties.Title) { }
    
    protected override void Validate(IPublishedContent content, string propertyAlias, TextBoxValueReq req) {
        if (req.Value.Length > MaxLength) {
            AddFailure<Strings>(propertyAlias, x => x.MaxLength, MaxLength);
        }
    }
    
    public override void PopulateContentPropertyCriteriaRes(IPropertyType property,
                                                            ContentPropertyCriteriaRes res) {
        var textBoxCriteria = new TextBoxCriteriaRes();
        textBoxCriteria.MaximumLength = MaxLength;
        textBoxCriteria.Description = property.Description;
        
        res.TextBox = textBoxCriteria;
    }
    
    public class Strings : ValidationStrings {
        public string MaxLength => $"Title cannot exceed {"{0}".Quote()} characters.";
    }
}
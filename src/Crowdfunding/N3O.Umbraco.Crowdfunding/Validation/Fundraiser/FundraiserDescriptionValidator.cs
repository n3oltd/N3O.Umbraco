using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.CrowdFunding;

public class FundraiserDescriptionValidator : ContentPropertyValidator<TextareaValueReq> {
    private const int MaxLength = 200;
    
    public FundraiserDescriptionValidator(IFormatter formatter)
        : base(formatter, CrowdfundingConstants.Fundraiser.Alias, CrowdfundingConstants.Fundraiser.Properties.Description) { }
    
    protected override void Validate(IPublishedContent content, string propertyAlias, TextareaValueReq req) {
        if (req.Value.Length > MaxLength) {
            AddFailure<Strings>(propertyAlias, x => x.MaxLength, MaxLength);
        }
    }
    
    public override void PopulateContentPropertyCriteriaRes(IPropertyType property,
                                                            PublishedDataType dataType,
                                                            ContentPropertyCriteriaRes res) {
        var textBoxCriteria = new TextareaCriteriaRes();
        textBoxCriteria.MaximumLength = MaxLength;
        textBoxCriteria.Description = property.Description;
        
        res.Textarea = textBoxCriteria;
    }
    
    public class Strings : ValidationStrings {
        public string MaxLength => $"Description cannot exceed {"{0}".Quote()} characters.";
    }
}
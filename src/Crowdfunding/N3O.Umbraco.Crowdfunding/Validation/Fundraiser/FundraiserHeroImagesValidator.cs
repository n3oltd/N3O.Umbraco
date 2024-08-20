using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.CrowdFunding;

public class FundraiserHeroImagesValidator : ContentPropertyValidator<NestedValueReq> {
    private const int MinItems = 1;
    private const int MaxItems = 5;
    
    public FundraiserHeroImagesValidator(IFormatter formatter)
        : base(formatter, CrowdfundingConstants.Fundraiser.Alias, CrowdfundingConstants.Fundraiser.Properties.HeroImages) { }
    
    protected override void Validate(IPublishedContent content, string propertyAlias, NestedValueReq req) {
        if (req.Items.Count() > MaxItems) {
            AddFailure<Strings>(propertyAlias, x => x.MaxItems, MaxItems);
        }
        
        if (req.Items.Count() < MinItems) {
            AddFailure<Strings>(propertyAlias, x => x.MinItems, MinItems);
        }
    }
    
    public override void PopulateContentPropertyCriteriaRes(IPropertyType property,
                                                            ContentPropertyCriteriaRes res) {
        var nestedCriteria = new NestedCriteriaRes();
        nestedCriteria.Description = property.Description;
        nestedCriteria.MaximumItems = MaxItems;
        nestedCriteria.MinimumItems = MinItems;
        
        res.Nested = nestedCriteria;
    }
    
    public class Strings : ValidationStrings {
        public string MaxItems => $"Only {"{0}".Quote()} images are allowed.";
        public string MinItems => $"At least {"{0}".Quote()} image should be specified.";
    }
}
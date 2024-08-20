using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.CrowdFunding;

public class FundraiserHeroImagesValidator : ContentPropertyValidator<NestedValueReq> {
    private const int MinItems = 1;
    
    public FundraiserHeroImagesValidator(IFormatter formatter)
        : base(formatter, CrowdfundingConstants.Fundraiser.Alias, CrowdfundingConstants.Fundraiser.Properties.HeroImages) { }
    
    protected override void Validate(IPublishedContent content, string propertyAlias, NestedValueReq req) {
        var property = content.Properties.SingleOrDefault(x => x.Alias == propertyAlias);
        var configuration = property?.PropertyType.DataType.ConfigurationAs<NestedContentConfiguration>();
        
        if (req.Items.Count() > configuration?.MaxItems) {
            AddFailure<Strings>(propertyAlias, x => x.MaxItems, configuration.MaxItems);
        }
        
        if (req.Items.Count() < MinItems) {
            AddFailure<Strings>(propertyAlias, x => x.MinItems, MinItems);
        }
    }
    
    public override void PopulateContentPropertyCriteriaRes(IPropertyType property,
                                                            PublishedDataType dataType,
                                                            ContentPropertyCriteriaRes res) {
        var configuration = dataType.ConfigurationAs<NestedContentConfiguration>();
        
        var nestedCriteria = new NestedCriteriaRes();
        nestedCriteria.Description = property.Description;
        nestedCriteria.MaximumItems = configuration.MaxItems.GetValueOrDefault();
        nestedCriteria.MinimumItems = MinItems;
        
        res.Nested = nestedCriteria;
    }
    
    public class Strings : ValidationStrings {
        public string MaxItems => $"Only {"{0}".Quote()} images are allowed.";
        public string MinItems => $"At least {"{0}".Quote()} image should be specified.";
    }
}
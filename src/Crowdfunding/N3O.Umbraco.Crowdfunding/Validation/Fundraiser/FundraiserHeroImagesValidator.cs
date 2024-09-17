using N3O.Umbraco.Data;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding;

public class FundraiserHeroImagesValidator : ContentPropertyValidator<NestedValueReq, NestedConfigurationRes> {
    private const int MinItems = 1;
    private const int MaxItems = 5;
    
    public FundraiserHeroImagesValidator(IFormatter formatter)
        : base(formatter, CrowdfundingConstants.Fundraiser.Alias, CrowdfundingConstants.Crowdfunder.Properties.HeroImages) { }
    
    protected override void PopulatePropertyConfiguration(IPropertyType property, NestedConfigurationRes res) {
        res.Description = property.Description;
        res.MaximumItems = MaxItems;
        res.MinimumItems = MinItems;
    }
    
    protected override void Validate(IPublishedContent content, string propertyAlias, NestedValueReq req) {
        if (req.Items.Count() > MaxItems) {
            AddFailure<Strings>(propertyAlias, x => x.MaxItems, MaxItems);
        }
        
        if (req.Items.Count() < MinItems) {
            AddFailure<Strings>(propertyAlias, x => x.MinItems, MinItems);
        }
    }
    
    public class Strings : ValidationStrings {
        public string MaxItems => $"Only {"{0}".Quote()} images are allowed.";
        public string MinItems => $"At least {"{0}".Quote()} image should be specified.";
    }
}
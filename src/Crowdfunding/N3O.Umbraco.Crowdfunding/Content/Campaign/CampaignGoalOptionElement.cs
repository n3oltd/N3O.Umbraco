using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CampaignGoalOptionElement : UmbracoElement<CampaignGoalOptionElement> {
    [UmbracoProperty(CrowdfundingConstants.Goal.Properties.Name)]
    public string Name => GetValue(x => x.Name);
    public IEnumerable<TagContent> Tags => GetPickedAs(x => x.Tags);
    public IEnumerable<PriceHandleElement> PriceHandles => GetNestedAs(x => x.PriceHandles);
    
    public string Id => Content().Key.ToString().ToLowerInvariant();
    
    public CampaignFundGoalOptionElement Fund { get; private set; }
    public CampaignFeedbackGoalOptionElement Feedback { get; private set; }
    
    public IFundDimensionOptions GetFundDimensionOptions(ILookups lookups) {
        var holdFundDimensionOptions = (IHoldFundDimensionOptions) Fund?.GetDonationItem(lookups) ??
                                       (IHoldFundDimensionOptions) Feedback?.GetScheme(lookups);

        return holdFundDimensionOptions.FundDimensionOptions;
    }
    
    public IEnumerable<FundDimension1Value> GetDimension1Values(ILookups lookups) {
        return GetLookups<FundDimension1Value>(lookups, CrowdfundingConstants.CampaignGoalOption.Properties.FundDimension1);
    }

    public IEnumerable<FundDimension2Value> GetDimension2Values(ILookups lookups) {
        return GetLookups<FundDimension2Value>(lookups, CrowdfundingConstants.CampaignGoalOption.Properties.FundDimension2);
    }

    public IEnumerable<FundDimension3Value> GetDimension3Values(ILookups lookups) {
        return GetLookups<FundDimension3Value>(lookups, CrowdfundingConstants.CampaignGoalOption.Properties.FundDimension3);
    }

    public IEnumerable<FundDimension4Value> GetDimension4Values(ILookups lookups) {
        return GetLookups<FundDimension4Value>(lookups, CrowdfundingConstants.CampaignGoalOption.Properties.FundDimension4);
    }
    
    public override void SetContent(IPublishedElement content, IPublishedContent parent) {
        base.SetContent(content, parent);
        
        if (Type == AllocationTypes.Fund) {
            Fund = new CampaignFundGoalOptionElement();
            Fund.SetContent(content, parent);
        } else if (Type == AllocationTypes.Feedback) {
            Feedback = new CampaignFeedbackGoalOptionElement();
            Feedback.SetContent(content, parent);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public override void SetVariationContext(VariationContext variationContext) {
        base.SetVariationContext(variationContext);
        
        Fund?.SetVariationContext(variationContext);
        Feedback?.SetVariationContext(variationContext);
    }
    
    public AllocationType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(CrowdfundingConstants.CampaignGoalOption.Fund.Alias)) {
                return AllocationTypes.Fund;
            } else if (Content().ContentType.Alias.EqualsInvariant(CrowdfundingConstants.CampaignGoalOption.Feedback.Alias)) {
                return AllocationTypes.Feedback;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
}
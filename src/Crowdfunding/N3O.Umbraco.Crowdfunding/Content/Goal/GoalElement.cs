using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Cloud.Engage.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

public class GoalElement : UmbracoElement<GoalElement>, ICrowdfunderGoal {
    [UmbracoProperty(CrowdfundingConstants.Goal.Properties.Name)]
    public string Name => GetValue(x => x.Name);
    public decimal Amount => GetValue(x => x.Amount);
    public string OptionId => GetValue(x => x.OptionId);
    public IEnumerable<TagContent> Tags => GetPickedAs(x => x.Tags);
    public IEnumerable<PriceHandleElement> PriceHandles => GetNestedAs(x => x.PriceHandles);
    
    public string Id => Content().Key.ToString().ToLowerInvariant();
    
    public FundGoalElement Fund { get; protected set; }
    public FeedbackGoalElement Feedback { get; protected set; }
    
    public bool HasPricing(ILookups lookups) {
        return ((IHoldPricing) Fund?.GetDonationItem(lookups) ?? Feedback?.GetScheme(lookups)).HasPricing();
    }

    public IFundDimensionValues GetFundDimensionValues(ILookups lookups) {
        return new FundDimensionValues(GetFundDimension1Value(lookups),
                                       GetFundDimension2Value(lookups),
                                       GetFundDimension3Value(lookups),
                                       GetFundDimension4Value(lookups));
    }

    IFundCrowdfunderGoal ICrowdfunderGoal.Fund => Fund;
    IFeedbackCrowdfunderGoal ICrowdfunderGoal.Feedback => Feedback;
    
    public override void SetContent(IPublishedElement content, IPublishedContent parent) {
        base.SetContent(content, parent);
        
        if (Type == AllocationTypes.Fund) {
            Fund = new FundGoalElement();
            Fund.SetContent(content, parent);
        } else if (Type == AllocationTypes.Feedback) {
            Feedback = new FeedbackGoalElement();
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
            if (Content().ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Goal.Fund.Alias)) {
                return AllocationTypes.Fund;
            } else if (Content().ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Goal.Feedback.Alias)) {
                return AllocationTypes.Feedback;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
    
    private FundDimension1Value GetFundDimension1Value(ILookups lookups) {
        return GetLookup<FundDimension1Value>(lookups, CrowdfundingConstants.Goal.Properties.FundDimension1);
    }

    private FundDimension2Value GetFundDimension2Value(ILookups lookups) {
        return GetLookup<FundDimension2Value>(lookups, CrowdfundingConstants.Goal.Properties.FundDimension1);
    }

    private FundDimension3Value GetFundDimension3Value(ILookups lookups) {
        return GetLookup<FundDimension3Value>(lookups, CrowdfundingConstants.Goal.Properties.FundDimension1);
    }

    private FundDimension4Value GetFundDimension4Value(ILookups lookups) {
        return GetLookup<FundDimension4Value>(lookups, CrowdfundingConstants.Goal.Properties.FundDimension1);
    }
}
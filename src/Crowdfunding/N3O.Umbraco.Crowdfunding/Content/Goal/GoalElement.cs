using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Engage.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

public class GoalElement : UmbracoElement<GoalElement>, IFundDimensionValues, ICrowdfunderGoal {
    [UmbracoProperty(CrowdfundingConstants.Goal.Properties.Name)]
    public string Name => GetValue(x => x.Name);
    public decimal Amount => GetValue(x => x.Amount);
    public string OptionId => GetValue(x => x.OptionId);
    public FundDimension1Value FundDimension1 => GetValue(x => x.FundDimension1);
    public FundDimension2Value FundDimension2 => GetValue(x => x.FundDimension2);
    public FundDimension3Value FundDimension3 => GetValue(x => x.FundDimension3);
    public FundDimension4Value FundDimension4 => GetValue(x => x.FundDimension4);
    public IEnumerable<TagContent> Tags => GetPickedAs(x => x.Tags);
    public IEnumerable<PriceHandleElement> PriceHandles => GetNestedAs(x => x.PriceHandles);
    
    public string Id => Content().Key.ToString().ToLowerInvariant();
    public bool HasPricing => ((IHoldPricing) Fund?.DonationItem ?? Feedback?.Scheme).HasPricing();
    
    public FundGoalElement Fund { get; protected set; }
    public FeedbackGoalElement Feedback { get; protected set; }

    public IFundDimensionValues FundDimensions => new FundDimensionValues(FundDimension1,
                                                                          FundDimension2,
                                                                          FundDimension3,
                                                                          FundDimension4);

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

    FundDimension1Value IFundDimensionValues.Dimension1 => FundDimension1;
    FundDimension2Value IFundDimensionValues.Dimension2 => FundDimension2;
    FundDimension3Value IFundDimensionValues.Dimension3 => FundDimension3;
    FundDimension4Value IFundDimensionValues.Dimension4 => FundDimension4;
}
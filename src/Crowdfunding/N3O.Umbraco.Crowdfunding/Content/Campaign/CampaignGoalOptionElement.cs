using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CampaignGoalOptionElement : UmbracoElement<CampaignGoalOptionElement>, IFundDimensionsOptions {
    [UmbracoProperty(CrowdfundingConstants.Goal.Properties.Name)]
    public string Name => GetValue(x => x.Name);
    public IEnumerable<FundDimension1Value> FundDimension1 => GetPickedAs(x => x.FundDimension1);
    public IEnumerable<FundDimension2Value> FundDimension2 => GetPickedAs(x => x.FundDimension2);
    public IEnumerable<FundDimension3Value> FundDimension3 => GetPickedAs(x => x.FundDimension3);
    public IEnumerable<FundDimension4Value> FundDimension4 => GetPickedAs(x => x.FundDimension4);
    public IEnumerable<TagContent> Tags => GetPickedAs(x => x.Tags);
    public IEnumerable<PriceHandleElement> PriceHandles => GetNestedAs(x => x.PriceHandles);
    
    public CampaignFundGoalOptionElement Fund { get; protected set; }
    public CampaignFeedbackGoalOptionElement Feedback { get; protected set; }
    
    public IFundDimensionsOptions GetFundDimensionOptions() {
        return (IFundDimensionsOptions) Fund?.DonationItem ??
               (IFundDimensionsOptions) Feedback?.Scheme;
    }
    
    public override void Content(IPublishedElement content, IPublishedContent parent) {
        base.Content(content, parent);
        
        if (Type == AllocationTypes.Fund) {
            Fund = new CampaignFundGoalOptionElement();
            Fund.Content(content, parent);
        } else if (Type == AllocationTypes.Feedback) {
            Feedback = new CampaignFeedbackGoalOptionElement();
            Feedback.Content(content, parent);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
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
    
    public Guid GoalId => Content().Key;

    IEnumerable<FundDimension1Value> IFundDimensionsOptions.Dimension1Options => FundDimension1;
    IEnumerable<FundDimension2Value> IFundDimensionsOptions.Dimension2Options => FundDimension2;
    IEnumerable<FundDimension3Value> IFundDimensionsOptions.Dimension3Options => FundDimension3;
    IEnumerable<FundDimension4Value> IFundDimensionsOptions.Dimension4Options => FundDimension4;
}
﻿using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
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
    
    public string Id => Content().Key.ToString().ToLowerInvariant();
    
    public CampaignFundGoalOptionElement Fund { get; private set; }
    public CampaignFeedbackGoalOptionElement Feedback { get; private set; }
    
    public IFundDimensionsOptions GetFundDimensionOptions() {
        return (IFundDimensionsOptions) Fund?.DonationItem ??
               (IFundDimensionsOptions) Feedback?.Scheme;
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

    IEnumerable<FundDimension1Value> IFundDimensionsOptions.Dimension1Options => FundDimension1;
    IEnumerable<FundDimension2Value> IFundDimensionsOptions.Dimension2Options => FundDimension2;
    IEnumerable<FundDimension3Value> IFundDimensionsOptions.Dimension3Options => FundDimension3;
    IEnumerable<FundDimension4Value> IFundDimensionsOptions.Dimension4Options => FundDimension4;
}
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.CrossSells.CompositionAlias)]
public class CrossSellContent : UmbracoContent<CrossSellContent>, IHoldCustomFormState {
    public string Name => Content().Name;
    public Guid Key => Content().Key;

    public MediaWithCrops Image => GetValue(x => x.Image);
    public MediaWithCrops Icon => GetValue(x => x.Icon);
    public IHtmlEncodedString Description => GetValue(x => x.Description);
    public ECommerceStage Stage => GetValue(x => x.Stage);
    public CampaignContent Targeting => GetAs(x => x.Targeting);
    public string CustomFormState => GetValue(x => x.CustomFormState);

    public OfferingType Type => GetValue(x => x.Type);
    public FundDimension1Value Dimension1 => GetValue(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetValue(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetValue(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetValue(x => x.Dimension4);
    public GiftType SuggestedGiftType => GetValue(x => x.SuggestedGiftType);
    public string NotesLabel => GetValue(x => x.NotesLabel);
    public DonationItem DonationItem => GetValue(x => x.DonationItem);
    public FeedbackScheme FeedbackScheme => GetValue(x => x.FeedbackScheme);
    public SponsorshipScheme SponsorshipScheme => GetValue(x => x.SponsorshipScheme);
    public IEnumerable<SuggestedAmountElement> OneTimeSuggestedAmounts => GetNestedAs(x => x.OneTimeSuggestedAmounts);
    public IEnumerable<SuggestedAmountElement> RecurringSuggestedAmounts => GetNestedAs(x => x.RecurringSuggestedAmounts);

    public IFundDimensionValues GetFixedFundDimensionValues() {
        return new FundDimensionValues(Dimension1, Dimension2, Dimension3, Dimension4);
    }
}
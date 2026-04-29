using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.CrossSells.CompositionAlias)]
public class CrossSellContent : UmbracoContent<CrossSellContent>, IHoldCustomFormState {
    public string Name => Content().Name;
    public Guid Key => Content().Key;

    public override void SetContent(IPublishedContent content) {
        base.SetContent(content);

        DonationFormContent = new DonationFormContent();
        DonationFormContent.SetContent(content);

        FormState = new DonationFormStateContent();
        FormState.SetContent(content);
    }

    public override void SetVariationContext(VariationContext variationContext) {
        base.SetVariationContext(variationContext);

        DonationFormContent?.SetVariationContext(variationContext);
        FormState?.SetVariationContext(variationContext);
    }

    public DonationFormContent DonationFormContent { get; private set; }
    public DonationFormStateContent FormState { get; private set; }

    public ECommerceStage Stage => GetValue(x => x.Stage);
    public CampaignContent Targeting => GetAs(x => x.Targeting);
    public string NotesLabel => GetValue(x => x.NotesLabel);
    public string CustomFormState => FormState.CustomFormState;
    public AllocationType Type => FormState.Type;
    public GiftType SuggestedGiftType => FormState.SuggestedGiftType;

    public IFundDimensionValues GetFixedFundDimensionValues() => FormState.GetFixedFundDimensionValues();
}

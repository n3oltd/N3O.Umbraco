using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Offerings.CompositionAlias)]
public class OfferingContent : UmbracoContent<OfferingContent>, IHoldCustomFormState {
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

    public string Name => Content().Name;
    public Guid Key => Content().Key;

    public string Notes => GetValue(x => x.Notes);
    public string NotesLabel => GetValue(x => x.NotesLabel);

    public string DonationButtonEmbedCode => GetValue(x => x.DonationButtonEmbedCode);
    public string DonationFormEmbedCode => GetValue(x => x.DonationFormEmbedCode);
    public string DonationPopupEmbedCode => GetValue(x => x.DonationPopupEmbedCode);

    public DonationFormContent DonationFormContent { get; private set; }
    public DonationFormStateContent FormState { get; private set; }

    public string CustomFormState => FormState.CustomFormState;
    public AllocationType Type => FormState.Type;
    public bool HasPricing => FormState.HasPricing;

    public IFundDimensionOptions GetFundDimensionOptions() => FormState.GetFundDimensionOptions();
    public IFundDimensionValues GetFixedFundDimensionValues() => FormState.GetFixedFundDimensionValues();
}

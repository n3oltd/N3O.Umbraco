using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Linq;
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
    public FundDimension1Value Dimension1 => GetValue(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetValue(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetValue(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetValue(x => x.Dimension4);
    public GiftType SuggestedGiftType => GetValue(x => x.SuggestedGiftType);
    public bool AllowCrowdfunding => GetValue(x => x.AllowCrowdfunding);
    public string CustomFormState => GetValue(x => x.CustomFormState);

    public string DonationButtonEmbedCode => GetValue(x => x.DonationButtonEmbedCode);
    public string DonationFormEmbedCode => GetValue(x => x.DonationFormEmbedCode);
    public string DonationPopupEmbedCode => GetValue(x => x.DonationPopupEmbedCode);

    public DonationFormContent DonationFormContent { get; private set; }
    public DonationFormStateContent FormState { get; private set; }

    public AllocationType Type => FormState.Type;
    public bool HasPricing => FormState.HasPricing;

    public IFundDimensionOptions GetFundDimensionOptions() => FormState.GetFundDimensionOptions();

    public IFundDimensionValues GetFixedFundDimensionValues() {
        if (Type == AllocationTypes.Qurbani) {
            return FormState.GetFixedFundDimensionValues();
        }

        var fundDimensionOptions = FormState.GetFundDimensionOptions();

        var dimension1 = Dimension1 ?? (fundDimensionOptions.Dimension1.IsSingle() ? fundDimensionOptions.Dimension1.Single() : null);
        var dimension2 = Dimension2 ?? (fundDimensionOptions.Dimension2.IsSingle() ? fundDimensionOptions.Dimension2.Single() : null);
        var dimension3 = Dimension3 ?? (fundDimensionOptions.Dimension3.IsSingle() ? fundDimensionOptions.Dimension3.Single() : null);
        var dimension4 = Dimension4 ?? (fundDimensionOptions.Dimension4.IsSingle() ? fundDimensionOptions.Dimension4.Single() : null);

        return new FundDimensionValues(dimension1, dimension2, dimension3, dimension4);
    }
}

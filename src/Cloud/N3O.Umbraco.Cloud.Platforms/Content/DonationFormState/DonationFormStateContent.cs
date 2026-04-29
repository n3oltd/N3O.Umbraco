using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.DonationFormStates.CompositionAlias)]
public class DonationFormStateContent : UmbracoContent<DonationFormStateContent>, IHoldCustomFormState {
    private static readonly string FeedbackDonationFormStateAlias = AliasHelper<FeedbackDonationFormStateContent>.ContentTypeAlias();
    private static readonly string FundDonationFormStateAlias = AliasHelper<FundDonationFormStateContent>.ContentTypeAlias();
    private static readonly string QurbaniDonationFormStateAlias = AliasHelper<QurbaniDonationFormStateContent>.ContentTypeAlias();
    private static readonly string SponsorshipDonationFormStateAlias = AliasHelper<SponsorshipDonationFormStateContent>.ContentTypeAlias();
    
    public override void SetContent(IPublishedContent content) {
        base.SetContent(content);
        
        if (Type == AllocationTypes.Fund) {
            Fund = new FundDonationFormStateContent();
            Fund.SetContent(content);
        } else if (Type == AllocationTypes.Feedback) {
            Feedback = new FeedbackDonationFormStateContent();
            Feedback.SetContent(content);
        } else if (Type == AllocationTypes.Qurbani) {
            Qurbani = new QurbaniDonationFormStateContent();
            Qurbani.SetContent(content);
        } else if (Type == AllocationTypes.Sponsorship) {
            Sponsorship = new SponsorshipDonationFormStateContent();
            Sponsorship.SetContent(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public override void SetVariationContext(VariationContext variationContext) {
        base.SetVariationContext(variationContext);
        
        Fund?.SetVariationContext(variationContext);
        Feedback?.SetVariationContext(variationContext);
        Qurbani?.SetVariationContext(variationContext);
        Sponsorship?.SetVariationContext(variationContext);
    }
    
    public string Name => Content().Name;
    public Guid Key => Content().Key;

    public FundDimension1Value Dimension1 => GetValue(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetValue(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetValue(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetValue(x => x.Dimension4);
    public GiftType SuggestedGiftType => GetValue(x => x.SuggestedGiftType);
    public bool AllowCrowdfunding => GetValue(x => x.AllowCrowdfunding);
    public string CustomFormState => GetValue(x => x.CustomFormState);
    
    public FundDonationFormStateContent Fund { get; private set; }
    public FeedbackDonationFormStateContent Feedback { get; private set; }
    public QurbaniDonationFormStateContent Qurbani { get; private set; }
    public SponsorshipDonationFormStateContent Sponsorship { get; private set; }
    
    public bool HasPricing {
        get {
            if (Type == AllocationTypes.Qurbani) {
                return Qurbani?.QurbaniItem?.Price != null;
            }
            return ((IHoldPricing) Fund?.DonationItem ?? Feedback?.Scheme).HasPricing();
        }
    }
    
    public IFundDimensionOptions GetFundDimensionOptions() {
        var holdFundDimensionOptions = (IHoldFundDimensionOptions) Fund?.DonationItem ??
                                       (IHoldFundDimensionOptions) Feedback?.Scheme ??
                                       (IHoldFundDimensionOptions) Sponsorship?.Scheme;

        return holdFundDimensionOptions.FundDimensionOptions;
    }
    
    public AllocationType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(FundDonationFormStateAlias)) {
                return AllocationTypes.Fund;
            } else if (Content().ContentType.Alias.EqualsInvariant(FeedbackDonationFormStateAlias)) {
                return AllocationTypes.Feedback;
            } else if (Content().ContentType.Alias.EqualsInvariant(QurbaniDonationFormStateAlias)) {
                return AllocationTypes.Qurbani;
            } else if (Content().ContentType.Alias.EqualsInvariant(SponsorshipDonationFormStateAlias)) {
                return AllocationTypes.Sponsorship;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }

    public IFundDimensionValues GetFixedFundDimensionValues() {
        if (Type == AllocationTypes.Qurbani) {
            return Qurbani?.QurbaniItem?.FundDimensionValues ?? new FundDimensionValues(null, null, null, null);
        }

        var fundDimensionOptions = GetFundDimensionOptions();

        var dimension1 = Dimension1 ?? (fundDimensionOptions.Dimension1.IsSingle() ? fundDimensionOptions.Dimension1.Single() : null);
        var dimension2 = Dimension2 ?? (fundDimensionOptions.Dimension2.IsSingle() ? fundDimensionOptions.Dimension2.Single() : null);
        var dimension3 = Dimension3 ?? (fundDimensionOptions.Dimension3.IsSingle() ? fundDimensionOptions.Dimension3.Single() : null);
        var dimension4 = Dimension4 ?? (fundDimensionOptions.Dimension4.IsSingle() ? fundDimensionOptions.Dimension4.Single() : null);

        return new FundDimensionValues(dimension1, dimension2, dimension3, dimension4);
    }
}
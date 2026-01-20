using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Offerings.CompositionAlias)]
public class OfferingContent : UmbracoContent<OfferingContent> {
    private static readonly string FeedbackOfferingAlias = AliasHelper<FeedbackOfferingContent>.ContentTypeAlias();
    private static readonly string FundOfferingAlias = AliasHelper<FundOfferingContent>.ContentTypeAlias();
    private static readonly string SponsorshipOfferingAlias = AliasHelper<SponsorshipOfferingContent>.ContentTypeAlias();
    
    public override void SetContent(IPublishedContent content) {
        base.SetContent(content);
        
        if (Type == OfferingTypes.Fund) {
            Fund = new FundOfferingContent();
            Fund.SetContent(content);
        } else if (Type == OfferingTypes.Feedback) {
            Feedback = new FeedbackOfferingContent();
            Feedback.SetContent(content);
        } else if (Type == OfferingTypes.Sponsorship) {
            Sponsorship = new SponsorshipOfferingContent();
            Sponsorship.SetContent(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public override void SetVariationContext(VariationContext variationContext) {
        base.SetVariationContext(variationContext);
        
        Fund?.SetVariationContext(variationContext);
        Feedback?.SetVariationContext(variationContext);
        Sponsorship?.SetVariationContext(variationContext);
    }
    
    public string Name => Content().Name;
    public Guid Key => Content().Key;

    public string Summary { get; set; }
    public string Notes => GetValue(x => x.Notes);
    public FundDimension1Value Dimension1 => GetValue(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetValue(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetValue(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetValue(x => x.Dimension4);
    public MediaWithCrops Icon => GetValue(x => x.Icon);
    public MediaWithCrops Image => GetValue(x => x.Image);
    public IHtmlEncodedString Description => GetValue(x => x.Description);
    public GiftType SuggestedGiftType => GetValue(x => x.SuggestedGiftType);
    public bool AllowCrowdfunding => GetValue(x => x.AllowCrowdfunding);
    
    public string DonationFormEmbedCode => GetValue(x => x.DonationFormEmbedCode);
    public string DonationButtonEmbedCode => GetValue(x => x.DonationButtonEmbedCode);
    
    public SponsorshipOfferingContent Sponsorship { get; private set; }
    public FundOfferingContent Fund { get; private set; }
    public FeedbackOfferingContent Feedback { get; private set; }
    
    public bool HasPricing => ((IHoldPricing) Fund?.DonationItem ?? Feedback?.Scheme).HasPricing();
    
    public IFundDimensionOptions GetFundDimensionOptions() {
        var holdFundDimensionOptions = (IHoldFundDimensionOptions) Fund?.DonationItem ??
                                       (IHoldFundDimensionOptions) Feedback?.Scheme ??
                                       (IHoldFundDimensionOptions) Sponsorship?.Scheme;

        return holdFundDimensionOptions.FundDimensionOptions;
    }
    
    public OfferingType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(FundOfferingAlias)) {
                return OfferingTypes.Fund;
            } else if (Content().ContentType.Alias.EqualsInvariant(FeedbackOfferingAlias)) {
                return OfferingTypes.Feedback;
            } else if (Content().ContentType.Alias.EqualsInvariant(SponsorshipOfferingAlias)) {
                return OfferingTypes.Sponsorship;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }

    public IFundDimensionValues GetFixedFundDimensionValues() {
        var fundDimensionOptions = GetFundDimensionOptions();
        
        var dimension1 = Dimension1 ?? (fundDimensionOptions.Dimension1.IsSingle() ? fundDimensionOptions.Dimension1.Single() : null);
        var dimension2 = Dimension2 ?? (fundDimensionOptions.Dimension2.IsSingle() ? fundDimensionOptions.Dimension2.Single() : null);
        var dimension3 = Dimension3 ?? (fundDimensionOptions.Dimension3.IsSingle() ? fundDimensionOptions.Dimension3.Single() : null);
        var dimension4 = Dimension4 ?? (fundDimensionOptions.Dimension4.IsSingle() ? fundDimensionOptions.Dimension4.Single() : null);
        
        return new FundDimensionValues(dimension1, dimension2, dimension3, dimension4);
    }
}
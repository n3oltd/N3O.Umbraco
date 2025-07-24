using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Designations.CompositionAlias)]
public class DesignationContent : UmbracoContent<DesignationContent> {
    private static readonly string FeedbackDesignationAlias = AliasHelper<FeedbackDesignationContent>.ContentTypeAlias();
    private static readonly string FundDesignationAlias = AliasHelper<FundDesignationContent>.ContentTypeAlias();
    private static readonly string SponsorshipDesignationAlias = AliasHelper<SponsorshipDesignationContent>.ContentTypeAlias();
    
    public override void SetContent(IPublishedContent content) {
        base.SetContent(content);
        
        if (Type == DesignationTypes.Fund) {
            Fund = new FundDesignationContent();
            Fund.SetContent(content);
        } else if (Type == DesignationTypes.Feedback) {
            Feedback = new FeedbackDesignationContent();
            Feedback.SetContent(content);
        } else if (Type == DesignationTypes.Sponsorship) {
            Sponsorship = new SponsorshipDesignationContent();
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

    public FundDimension1Value Dimension1 => GetValue(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetValue(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetValue(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetValue(x => x.Dimension4);
    public MediaWithCrops Icon => GetValue(x => x.Icon);
    public MediaWithCrops Image => GetValue(x => x.Image);
    public IHtmlEncodedString LongDescription => GetValue(x => x.LongDescription);
    public IHtmlEncodedString ShortDescription => GetValue(x => x.ShortDescription);
    public GiftType SuggestedGiftType => GetValue(x => x.SuggestedGiftType);
    
    public SponsorshipDesignationContent Sponsorship { get; private set; }
    public FundDesignationContent Fund { get; private set; }
    public FeedbackDesignationContent Feedback { get; private set; }
    
    public bool HasPricing => ((IHoldPricing) Fund?.DonationItem ?? Feedback?.Scheme).HasPricing();
    
    public IReadOnlyList<GiftType> GetGiftTypes() {
        var givingTypes = Fund?.DonationItem.AllowedGivingTypes ??
                          Feedback?.Scheme.AllowedGivingTypes ??
                          Sponsorship?.Scheme.AllowedGivingTypes;

        if (givingTypes.HasAny()) {
            return givingTypes.Select(x => x.ToGiftType()).ToList();
        } else {
            return null;
        }
    }
    
    public IFundDimensionOptions GetFundDimensionOptions() {
        var holdFundDimensionOptions = (IHoldFundDimensionOptions) Fund?.DonationItem ??
                                       (IHoldFundDimensionOptions) Feedback?.Scheme ??
                                       (IHoldFundDimensionOptions) Sponsorship?.Scheme;

        return holdFundDimensionOptions.FundDimensionOptions;
    }
    
    public DesignationType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(FundDesignationAlias)) {
                return DesignationTypes.Fund;
            } else if (Content().ContentType.Alias.EqualsInvariant(FeedbackDesignationAlias)) {
                return DesignationTypes.Feedback;
            } else if (Content().ContentType.Alias.EqualsInvariant(SponsorshipDesignationAlias)) {
                return DesignationTypes.Sponsorship;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
}
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Designations.CompositionAlias)]
public class DesignationContent : UmbracoContent<DesignationContent> {
    private const string Dimension1Alias = PlatformsConstants.Designations.Properties.Dimension1;
    private const string Dimension2Alias = PlatformsConstants.Designations.Properties.Dimension2;
    private const string Dimension3Alias = PlatformsConstants.Designations.Properties.Dimension3;
    private const string Dimension4Alias = PlatformsConstants.Designations.Properties.Dimension4;

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
    public MediaWithCrops Icon => GetValue(x => x.Icon);
    public MediaWithCrops Image => GetValue(x => x.Image);
    public IHtmlEncodedString LongDescription => GetValue(x => x.LongDescription);
    public IHtmlEncodedString ShortDescription => GetValue(x => x.ShortDescription);
    public GiftType SuggestedGiftType => GetValue(x => x.SuggestedGiftType);
    
    public SponsorshipDesignationContent Sponsorship { get; private set; }
    public FundDesignationContent Fund { get; private set; }
    public FeedbackDesignationContent Feedback { get; private set; }

    public bool HasPricing(ILookups lookups) {
        return ((IHoldPricing) Fund?.GetDonationItem(lookups) ?? Feedback?.GetScheme(lookups)).HasPricing();
    }
    
    public IFundDimensionValues GetFundDimensionValues(ILookups lookups) {
        return new FundDimensionValues(GetFundDimension1Value(lookups),
                                       GetFundDimension2Value(lookups),
                                       GetFundDimension3Value(lookups),
                                       GetFundDimension4Value(lookups));
    }
    
    public IReadOnlyList<GiftType> GetGiftTypes(ILookups lookups) {
        var givingTypes = Fund?.GetDonationItem(lookups).AllowedGivingTypes ??
                          Feedback?.GetScheme(lookups).AllowedGivingTypes ??
                          Sponsorship?.GetScheme(lookups).AllowedGivingTypes;

        if (givingTypes.HasAny()) {
            return givingTypes.Select(x => x.ToGiftType()).ToList();
        } else {
            return null;
        }
    }
    
    public IFundDimensionOptions GetFundDimensionOptions(ILookups lookups) {
        var holdFundDimensionOptions = (IHoldFundDimensionOptions) Fund?.GetDonationItem(lookups) ??
                                       (IHoldFundDimensionOptions) Feedback?.GetScheme(lookups) ??
                                       (IHoldFundDimensionOptions) Sponsorship?.GetScheme(lookups);

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
    
    private FundDimension1Value GetFundDimension1Value(ILookups lookups) {
        return GetLookup<FundDimension1Value>(lookups, Dimension1Alias);
    }

    private FundDimension2Value GetFundDimension2Value(ILookups lookups) {
        return GetLookup<FundDimension2Value>(lookups, Dimension2Alias);
    }

    private FundDimension3Value GetFundDimension3Value(ILookups lookups) {
        return GetLookup<FundDimension3Value>(lookups, Dimension3Alias);
    }

    private FundDimension4Value GetFundDimension4Value(ILookups lookups) {
        return GetLookup<FundDimension4Value>(lookups, Dimension4Alias);
    }
}
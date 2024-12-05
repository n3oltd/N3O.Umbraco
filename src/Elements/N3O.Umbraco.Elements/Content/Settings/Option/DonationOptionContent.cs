using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Elements.Content;

public class DonationOptionContent : UmbracoContent<DonationOptionContent>, IFundDimensionValues {
    private static readonly string FeedbackAlias = AliasHelper<FeedbackDonationOptionContent>.ContentTypeAlias();
    private static readonly string FundAlias = AliasHelper<FundDonationOptionContent>.ContentTypeAlias();
    private static readonly string SponsorshipAlias = AliasHelper<SponsorshipDonationOptionContent>.ContentTypeAlias();

    public override void Content(IPublishedContent content) {
        base.Content(content);
        
        if (Type == AllocationTypes.Feedback) {
            Feedback = new FeedbackDonationOptionContent();
            Feedback.Content(content);
        } else if (Type == AllocationTypes.Fund) {
            Fund = new FundDonationOptionContent();
            Fund.Content(content);
        } else if (Type == AllocationTypes.Sponsorship) {
            Sponsorship = new SponsorshipDonationOptionContent();
            Sponsorship.Content(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }

    public Guid Id => Content().Key;
    public string Name => Content()?.Name;
    public MediaWithCrops Image => GetValue(x => x.Image);
    public DonationCategoryContent PrimaryCategory => GetAs(x => x.PrimaryCategory);
    public IEnumerable<DonationCategoryContent> AdditionalCategories => GetPickedAs(x => x.AdditionalCategories);
    public GivingType DefaultGivingType => GetValue(x => x.DefaultGivingType);
    public FundDimension1Value Dimension1 => GetAs(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetAs(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetAs(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetAs(x => x.Dimension4);
    public bool DefaultOptionInCategory => GetValue(x => x.DefaultOptionInCategory);
    public bool HideQuantity => GetValue(x => x.HideQuantity);
    public bool HideDonation => GetValue(x => x.HideDonation);
    public bool HideRegularGiving => GetValue(x => x.HideRegularGiving);
    public string Synopsis => GetValue(x => x.Synopsis);
    public string Description => GetValue(x => x.Description);
    
    public string ImageUrl => Image.GetCropUrl(ElementsConstants.DonationOption.CropAlias);
    
    public IEnumerable<DonationCategoryContent> AllCategories => PrimaryCategory.Yield().Concat(AdditionalCategories.OrEmpty());

    public FeedbackDonationOptionContent Feedback { get; private set; }
    public FundDonationOptionContent Fund { get; private set; }
    public SponsorshipDonationOptionContent Sponsorship { get; private set; }

    public IFundDimensionsOptions GetFundDimensionOptions() {
        return (IFundDimensionsOptions) Fund?.DonationItem ??
               (IFundDimensionsOptions) Sponsorship?.Scheme ??
               (IFundDimensionsOptions) Feedback?.Scheme;
    }

    public bool IsValid() {
        if (Type == AllocationTypes.Fund) {
            return Fund.IsValid();
        } else if (Type == AllocationTypes.Sponsorship) {
            return Sponsorship.IsValid();
        } else if (Type == AllocationTypes.Feedback) {
            return Feedback.IsValid();
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public AllocationType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(FundAlias)) {
                return AllocationTypes.Fund;
            } else if (Content().ContentType.Alias.EqualsInvariant(SponsorshipAlias)) {
                return AllocationTypes.Sponsorship;
            } else if (Content().ContentType.Alias.EqualsInvariant(FeedbackAlias)) {
                return AllocationTypes.Feedback;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
}

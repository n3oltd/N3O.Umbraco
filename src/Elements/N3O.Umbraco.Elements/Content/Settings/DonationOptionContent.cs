using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Lookups;
using N3O.Umbraco.Elements.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Elements.Content;

public class DonationOptionContent : UmbracoContent<DonationOptionContent>, IFundDimensionValues {
    private static readonly string FundDonationOptionAlias = AliasHelper<FundDonationOptionContent>.ContentTypeAlias();
    private static readonly string SponsorshipDonationOptionAlias = AliasHelper<SponsorshipDonationOptionContent>.ContentTypeAlias();
    private static readonly string FeedbackDonationOptionAlias = AliasHelper<FeedbackDonationOptionContent>.ContentTypeAlias();

    public override void Content(IPublishedContent content) {
        base.Content(content);
        
        if (Type == AllocationTypes.Fund) {
            Fund = new FundDonationOptionContent();
            Fund.Content(content);
        } else if (Type == AllocationTypes.Sponsorship) {
            Sponsorship = new SponsorshipDonationOptionContent();
            Sponsorship.Content(content);
        } else if (Type == AllocationTypes.Feedback) {
            Feedback = new FeedbackDonationOptionContent();
            Feedback.Content(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }

    public int Id => Content().Id;
    public string Name => Content()?.Name;
    public DonationCategoryBaseContent PrimaryCategory => GetAs(x => x.PrimaryCategory);
    public IEnumerable<DonationCategoryBaseContent> AdditionalCategories => GetPickedAs(x => x.AdditionalCategories);
    public GivingType DefaultGivingType => GetValue(x => x.DefaultGivingType);
    public FundDimension1Value Dimension1 => GetAs(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetAs(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetAs(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetAs(x => x.Dimension4);
    public bool DefaultOption => GetValue(x => x.DefaultOption);
    public bool HideQuantity => GetValue(x => x.HideQuantity);
    public bool HideDonation => GetValue(x => x.HideDonation);
    public bool HideRegularGiving => GetValue(x => x.HideRegularGiving);
    public MediaWithCrops Image => GetValue(x => x.Image);
    public string Title => GetValue(x => x.Title);
    public string Description => GetValue(x => x.Description);
    
    public IEnumerable<DonationCategoryBaseContent> Categories => AdditionalCategories.Concat(PrimaryCategory);

    public FundDonationOptionContent Fund { get; private set; }
    public SponsorshipDonationOptionContent Sponsorship { get; private set; }
    public FeedbackDonationOptionContent Feedback { get; private set; }

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
            if (Content().ContentType.Alias.EqualsInvariant(FundDonationOptionAlias)) {
                return AllocationTypes.Fund;
            } else if (Content().ContentType.Alias.EqualsInvariant(SponsorshipDonationOptionAlias)) {
                return AllocationTypes.Sponsorship;
            } else if (Content().ContentType.Alias.EqualsInvariant(FeedbackDonationOptionAlias)) {
                return AllocationTypes.Feedback;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
    
    public object ToFormJson(IJsonProvider jsonProvider) {
        var option = new {
            Id = Content().Key,
            Name,
            Image = Image.Url(),
            Title,
            Description,
            DefaultCategoryId = PrimaryCategory.Content().Key,
            AdditionalCategoriesIds = AdditionalCategories?.Select(x => x.Content().Key),
            DefaultGivingType,
            HideQuantity,
            HideDonation,
            HideRegularGiving,
            Dimension1,
            Dimension2,
            Dimension3,
            Dimension4,
            AllocationType = Type,
            Fund,
            Sponsorship,
            Feedback
        };
        
        return jsonProvider.SerializeObject(option);
    }
}

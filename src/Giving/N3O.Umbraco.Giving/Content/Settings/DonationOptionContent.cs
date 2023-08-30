using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Content;

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
    public string CampaignName => GetCampaignName();
    public GivingType DefaultGivingType => GetValue(x => x.DefaultGivingType);
    public FundDimension1Value Dimension1 => GetAs(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetAs(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetAs(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetAs(x => x.Dimension4);
    public bool HideQuantity => GetValue(x => x.HideQuantity);
    public bool HideDonation => GetValue(x => x.HideDonation);
    public bool HideRegularGiving => GetValue(x => x.HideRegularGiving);

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
    
    [JsonIgnore]
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
    
    private string GetCampaignName() {
        var parent = Content()?.Parent;
        
        if (parent?.ContentType.Alias.EqualsInvariant(GivingConstants.Aliases.DonationCampaign.ContentType) == true) {
            return parent.Name;
        } else {
            return null;
        }
    }
}

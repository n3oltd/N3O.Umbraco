using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Giving.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Content;

public class DonationOptionContent : UmbracoContent<DonationOptionContent>, IFundDimensionValues {
    private static readonly string FundDonationOptionAlias = AliasHelper<FundDonationOptionContent>.ContentTypeAlias();
    private static readonly string SponsorshipDonationOptionAlias = AliasHelper<SponsorshipDonationOptionContent>.ContentTypeAlias();

    public override void Content(IPublishedContent value) {
        base.Content(value);
        
        if (Type == AllocationTypes.Fund) {
            Fund = new FundDonationOptionContent();
            Fund.Content(value);
        } else if (Type == AllocationTypes.Sponsorship) {
            Sponsorship = new SponsorshipDonationOptionContent();
            Sponsorship.Content(value);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }

    public string Name => Content()?.Name;
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

    public IFundDimensionsOptions GetFundDimensionOptions() {
        return (IFundDimensionsOptions) Fund?.DonationItem ?? Sponsorship?.Scheme;
    }

    public bool IsValid() {
        if (Type == AllocationTypes.Fund) {
            return Fund.IsValid();
        } else if (Type == AllocationTypes.Sponsorship) {
            return Sponsorship.IsValid();
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
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
}

using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Content;

public class UpsellOfferValidator : ContentValidator {
    private static readonly string UpsellOfferContentContentTypeAlias = AliasHelper<UpsellOfferContent>.ContentTypeAlias();
    private static readonly string Dimension1Alias = AliasHelper<UpsellOfferContent>.PropertyAlias(x => x.Dimension1);
    private static readonly string Dimension2Alias = AliasHelper<UpsellOfferContent>.PropertyAlias(x => x.Dimension2);
    private static readonly string Dimension3Alias = AliasHelper<UpsellOfferContent>.PropertyAlias(x => x.Dimension3);
    private static readonly string Dimension4Alias = AliasHelper<UpsellOfferContent>.PropertyAlias(x => x.Dimension4);
    private static readonly string DonationItemAlias = AliasHelper<UpsellOfferContent>.PropertyAlias(x => x.DonationItem);
    private static readonly string FixedAmount = AliasHelper<UpsellOfferContent>.PropertyAlias(x => x.FixedAmount);
    private static readonly string GivingType = AliasHelper<UpsellOfferContent>.PropertyAlias(x => x.GivingType);
    private static readonly string PriceHandles = AliasHelper<UpsellOfferContent>.PropertyAlias(x => x.PriceHandles);

    public UpsellOfferValidator(IContentHelper contentHelper) : base(contentHelper) { }

    public override bool IsValidator(ContentProperties content) {
        return content.ContentTypeAlias.Equals(UpsellOfferContentContentTypeAlias);
    }

    public override void Validate(ContentProperties content) {
        var donationItem = GetDonationItem(content);
        
        if (donationItem == null) {
            ErrorResult("Donation item must be specified");
        } else {
            ValidateFundDimensions(content, donationItem);
            ValidateGivingType(content, donationItem);
            ValidatePricing(content, donationItem);
        }
    }

    private void ValidateFundDimensions(ContentProperties content, DonationItem donationItem) {
        DimensionAllowed(content, donationItem.Dimension1Options, Dimension1Alias);
        DimensionAllowed(content, donationItem.Dimension2Options, Dimension2Alias);
        DimensionAllowed(content, donationItem.Dimension3Options, Dimension3Alias);
        DimensionAllowed(content, donationItem.Dimension4Options, Dimension4Alias);
    }
    
    private void ValidateGivingType(ContentProperties content, DonationItem donationItem) {
        var givingType = content.GetPropertyByAlias(GivingType)
                                  .IfNotNull(x => ContentHelper.GetDataListValue<GivingType>(x));

        if (givingType!= null && !donationItem.AllowedGivingTypes.HasAny(x => x == givingType)) {
            ErrorResult("Donation item does not allow specified giving type");
        }
    }
    
    private void ValidatePricing(ContentProperties content, DonationItem donationItem) {
        var hasFixedAmount = content.GetPropertyByAlias(FixedAmount).Value.HasValue();
        var hasPriceHandles = content.GetPropertyByAlias(PriceHandles).Value.HasValue();

        if (donationItem.HasPricing() && hasFixedAmount) {
            ErrorResult("Donation item has pricing so a fixed amount cannot be specified");
        }
        
        if (donationItem.HasPricing() && hasPriceHandles) {
            ErrorResult("Donation item has pricing so price handles cannot be specified");
        }

        if (hasFixedAmount && hasPriceHandles) {
            ErrorResult("A fixed amount and price handles cannot both be specified");
        }
    }

    private DonationItem GetDonationItem(ContentProperties content) {
        var donationItem = content.GetPropertyByAlias(DonationItemAlias)
                                  .IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x)
                                                               .As<DonationItem>());

        return donationItem;
    }

    private void DimensionAllowed<T>(ContentProperties content, IEnumerable<T> allowedValues, string propertyAlias)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x).As<T>());

        if (value != null && allowedValues != null && !allowedValues.Contains(value)) {
            ErrorResult(property, $"{value.Name} is not a permitted fund dimension value");
        }
    }
}
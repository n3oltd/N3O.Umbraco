using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Allocations.Content;

public class UpsellOfferValidator : ContentValidator {
    private static readonly string UpsellOfferContentContentTypeAlias = AliasHelper<UpsellOfferContent>.ContentTypeAlias();
    private static readonly string FixedAmount = AliasHelper<UpsellOfferContent>.PropertyAlias(x => x.FixedAmount);
    private static readonly string GivingType = AliasHelper<UpsellOfferContent>.PropertyAlias(x => x.GivingType);
    private static readonly string PriceHandles = AliasHelper<UpsellOfferContent>.PropertyAlias(x => x.PriceHandles);
    private readonly ILookups _lookups;

    public UpsellOfferValidator(IContentHelper contentHelper, ILookups lookups) : base(contentHelper) {
        _lookups = lookups;
    }

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
        DimensionAllowed(content, donationItem.FundDimensionOptions.Dimension1, AllocationsConstants.Aliases.UpsellOffer.Properties.Dimension1);
        DimensionAllowed(content, donationItem.FundDimensionOptions.Dimension2, AllocationsConstants.Aliases.UpsellOffer.Properties.Dimension2);
        DimensionAllowed(content, donationItem.FundDimensionOptions.Dimension3, AllocationsConstants.Aliases.UpsellOffer.Properties.Dimension3);
        DimensionAllowed(content, donationItem.FundDimensionOptions.Dimension4, AllocationsConstants.Aliases.UpsellOffer.Properties.Dimension4);
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
        var donationItem = content.GetPropertyByAlias(AllocationsConstants.Aliases.UpsellOffer.Properties.DonationItem)
                                  .IfNotNull(x => ContentHelper.GetLookupValue<DonationItem>(_lookups, x));

        return donationItem;
    }

    private void DimensionAllowed<T>(ContentProperties content, IEnumerable<T> allowedValues, string propertyAlias)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetLookupValue<T>(_lookups, x));

        if (value != null && allowedValues != null && !allowedValues.Contains(value)) {
            ErrorResult(property, $"{value.Name} is not a permitted fund dimension value");
        }
    }
}
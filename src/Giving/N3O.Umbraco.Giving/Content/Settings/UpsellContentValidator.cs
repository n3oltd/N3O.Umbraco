using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Content;

public class UpsellContentValidator : ContentValidator {
    private static readonly string UpsellContentContentTypeAlias = AliasHelper<UpsellContent>.ContentTypeAlias();
    private static readonly string Dimension1Alias = AliasHelper<UpsellContent>.PropertyAlias(x => x.Dimension1);
    private static readonly string Dimension2Alias = AliasHelper<UpsellContent>.PropertyAlias(x => x.Dimension2);
    private static readonly string Dimension3Alias = AliasHelper<UpsellContent>.PropertyAlias(x => x.Dimension3);
    private static readonly string Dimension4Alias = AliasHelper<UpsellContent>.PropertyAlias(x => x.Dimension4);
    private static readonly string DonationItemAlias = AliasHelper<UpsellContent>.PropertyAlias(x => x.DonationItem);
    private static readonly string PricingMode = AliasHelper<UpsellContent>.PropertyAlias(x => x.PricingMode);
    private static readonly string FixedAmount = AliasHelper<UpsellContent>.PropertyAlias(x => x.FixedAmount);
    private static readonly string PriceHandles = AliasHelper<UpsellContent>.PropertyAlias(x => x.PriceHandles);

    public UpsellContentValidator(IContentHelper contentHelper) : base(contentHelper) { }

    public override bool IsValidator(ContentProperties content) {
        return content.ContentTypeAlias.Equals(UpsellContentContentTypeAlias);
    }

    public override void Validate(ContentProperties content) {
        var donationItem = GetDonationItem(content);
        
        ValidateFundDimensions(content, donationItem);
        ValidatePricing(content, donationItem);
    }

    private void ValidateFundDimensions(ContentProperties content, DonationItem donationItem) {
        if (donationItem != null) {
            DimensionAllowed(content, donationItem.Dimension1Options, Dimension1Alias);
            DimensionAllowed(content, donationItem.Dimension2Options, Dimension2Alias);
            DimensionAllowed(content, donationItem.Dimension3Options, Dimension3Alias);
            DimensionAllowed(content, donationItem.Dimension4Options, Dimension4Alias);
        }
    }
    
    private void ValidatePricing(ContentProperties content, DonationItem donationItem) {
        var pricingMode = content.GetPropertyByAlias(PricingMode)
                                 .IfNotNull(x => ContentHelper.GetDataListValue<UpsellPricingMode>(x));

        if (donationItem.HasPricing() && pricingMode != UpsellPricingModes.DonationItem) {
            ErrorResult($"Donation item has pricing, cannot select {pricingMode.Name} as Pricing Mode");
        }

        if (pricingMode == UpsellPricingModes.DonationItem && !donationItem.HasPricing()) {
            ErrorResult($"Donation item does not has pricing, cannot select {pricingMode.Name} as Pricing Mode");
        }
        
        if (pricingMode == UpsellPricingModes.AnyAmount ||
            pricingMode == UpsellPricingModes.Custom ||
            pricingMode == UpsellPricingModes.DonationItem) {
            if (content.GetPropertyByAlias(FixedAmount).Value.HasValue() ||
                content.GetPropertyByAlias(PriceHandles).Value.HasValue()) {
                ErrorResult($"Fixed amount and price handles cannot be specified with {pricingMode.Name} Price mode");
            }
        }
        
        if(pricingMode == UpsellPricingModes.FixedAmount) {
            if (content.GetPropertyByAlias(PriceHandles).Value.HasValue()) {
                ErrorResult($"Price handles cannot be specified with {pricingMode.Name} Price mode");
            }

            if (!content.GetPropertyByAlias(FixedAmount).Value.HasValue()) {
                ErrorResult($"Please specify Fixed Amount for {pricingMode.Name} Price mode");
            }
            
        }
        
        if (pricingMode == UpsellPricingModes.PriceHandles && content.GetPropertyByAlias(FixedAmount).Value.HasValue()) {
            if (content.GetPropertyByAlias(FixedAmount).Value.HasValue()) {
                ErrorResult($"Price handles cannot be specified with {pricingMode.Name} Price mode");
            }

            if (!content.GetPropertyByAlias(PriceHandles).Value.HasValue()) {
                ErrorResult($"Please specify Price handles for {pricingMode.Name} Price mode");
            }
        }
    }

    private DonationItem GetDonationItem(ContentProperties content) {
        var donationItem = content.GetPropertyByAlias(DonationItemAlias)
                                  .IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x)
                                                               .As<DonationItem>());

        return donationItem;
    }

    private void DimensionAllowed<T>(ContentProperties content,
                                     IEnumerable<T> allowedValues,
                                     string propertyAlias)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x)
                                                         .As<T>());

        if (value != null && allowedValues != null && !allowedValues.Contains(value)) {
            ErrorResult(property, $"{value.Name} is not a permitted fund dimension value");
        }
    }
}
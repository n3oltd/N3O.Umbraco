using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Extensions;

public static class UpsellOfferContentExtensions {
    public static async Task<Allocation> ToAllocationAsync(this UpsellOfferContent upsellOfferContent,
                                                           IForexConverter forexConverter,
                                                           IPriceCalculator priceCalculator,
                                                           Currency currency,
                                                           decimal? customAmount,
                                                           GivingType givingType,
                                                           Money cartTotal) {
        if (upsellOfferContent == null) {
            return null;
        }

        var amount = await CalculatePriceAsync(forexConverter,
                                               priceCalculator,
                                               upsellOfferContent,
                                               currency,
                                               givingType,
                                               cartTotal)
                     ?? customAmount.IfNotNull(x => new Money(x, currency));

        if (amount == null) {
            throw new Exception("Amount must be specified for upsell");
        }

        return new Allocation(AllocationTypes.Fund,
                              amount,
                              upsellOfferContent.FundDimensions,
                              new FundAllocation(upsellOfferContent.DonationItem),
                              null,
                              null,
                              upsellOfferContent.Content().Key);
    }

    public static async Task<UpsellOffer> ToUpsellOfferAsync(this UpsellOfferContent upsellOfferContent,
                                                             IForexConverter forexConverter,
                                                             IPriceCalculator priceCalculator,
                                                             Currency currency,
                                                             GivingType givingType,
                                                             Money cartTotal) {
        if (upsellOfferContent == null) {
            return null;
        }

        var price = await CalculatePriceAsync(forexConverter,
                                              priceCalculator,
                                              upsellOfferContent,
                                              currency,
                                              givingType,
                                              cartTotal);

        return new UpsellOffer(upsellOfferContent.AllowMultiple,
                               upsellOfferContent.Content().Key,
                               upsellOfferContent.Content().Name,
                               upsellOfferContent.Description,
                               upsellOfferContent.OfferedFor,
                               upsellOfferContent.GivingType,
                               price,
                               upsellOfferContent.PriceHandles);
    }

    private static async Task<Money> CalculatePriceAsync(IForexConverter forexConverter,
                                                         IPriceCalculator priceCalculator,
                                                         UpsellOfferContent upsellOfferContent,
                                                         Currency currency,
                                                         GivingType givingType,
                                                         Money cartTotal) {
        Money price = null;

        if (upsellOfferContent.DonationItem.HasPricing()) {
            price = new Money((await priceCalculator.InCurrencyAsync(upsellOfferContent.DonationItem,
                                                                     upsellOfferContent.FundDimensions,
                                                                     currency)).Amount,
                              currency);
        } else if (upsellOfferContent.FixedAmount.GetValueOrDefault() != default) {
            price = (await forexConverter.BaseToQuote()
                                         .ToCurrency(currency)
                                         .ConvertAsync(upsellOfferContent.FixedAmount.GetValueOrThrow())).Quote;
        } else if (!upsellOfferContent.PriceHandles.HasAny()) {
            price = GetCustomPrice(forexConverter, upsellOfferContent, currency, givingType, cartTotal);
        }

        return price;
    }

    private static Money GetCustomPrice(IForexConverter forexConverter,
                                        UpsellOfferContent upsellOfferContent,
                                        Currency currency,
                                        GivingType givingType,
                                        Money cartTotal) {
        var customUpsellPricingType = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                                  t.HasParameterlessConstructor() &&
                                                                  t.ImplementsInterface<ICustomUpsellPricing>())
                                                   .SingleOrDefault();

        if (customUpsellPricingType == null) {
            return null;
        }

        var customUpsellPricing = (ICustomUpsellPricing) Activator.CreateInstance(customUpsellPricingType);

        return customUpsellPricing.GetPrice(forexConverter, upsellOfferContent, currency, givingType, cartTotal);
    }
}
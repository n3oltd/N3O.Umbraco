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

public static class UpsellContentExtensions {
    public static async Task<Allocation> ToAllocationAsync(this UpsellContent upsellContent,
                                                           IForexConverter forexConverter,
                                                           IPriceCalculator priceCalculator,
                                                           Currency currency,
                                                           decimal? customAmount,
                                                           Money cartTotal) {
        if (upsellContent == null) {
            return null;
        }
        
        var amount = await CalculatePriceAsync(forexConverter, priceCalculator, upsellContent, currency, cartTotal)
                     ?? customAmount.IfNotNull(x => new Money(x, currency));

        if (amount == null) {
            throw new Exception("Amount must be specified for upsell");
        }

        return new Allocation(AllocationTypes.Fund,
                              amount,
                              upsellContent.FundDimensions,
                              new FundAllocation(upsellContent.DonationItem),
                              null,
                              null,
                              upsellContent.Content().Key);
    }
    
    public static async Task<UpsellOffer> ToUpsellOfferAsync(this UpsellContent upsellContent,
                                                             IForexConverter forexConverter,
                                                             IPriceCalculator priceCalculator,
                                                             Currency currency,
                                                             Money cartTotal) {
        if (upsellContent == null) {
            return null;
        }
        
        var price = await CalculatePriceAsync(forexConverter, priceCalculator, upsellContent, currency, cartTotal);

        return new UpsellOffer(upsellContent.Content().Key,
                               upsellContent.Content().Name,
                               upsellContent.Description,
                               price,
                               upsellContent.PriceHandles);
    }

    private static async Task<Money> CalculatePriceAsync(IForexConverter forexConverter,
                                                         IPriceCalculator priceCalculator,
                                                         UpsellContent upsellContent,
                                                         Currency currency,
                                                         Money cartTotal) {
        Money price;

        if (upsellContent.DonationItem.HasPricing()) {
            price = new Money((await priceCalculator.InCurrencyAsync(upsellContent.DonationItem,
                                                                     upsellContent.FundDimensions,
                                                                     currency)).Amount,
                              currency);
        } else if (upsellContent.FixedAmount.HasValue()) {
            price = (await forexConverter.BaseToQuote()
                                         .ToCurrency(currency)
                                         .ConvertAsync(upsellContent.FixedAmount.GetValueOrThrow())).Quote;
        } else {
            price = GetCustomPrice(forexConverter, upsellContent, currency, cartTotal);
        }

        return price;
    }

    private static Money GetCustomPrice(IForexConverter forexConverter,
                                        UpsellContent upsellContent,
                                        Currency currency,
                                        Money cartTotal) {
        var customUpsellPricingType = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                                  t.HasParameterlessConstructor() &&
                                                                  t.ImplementsInterface<ICustomUpsellPricing>())
                                                   .SingleOrDefault();

        if (customUpsellPricingType == null) {
            return null;
        }

        var customUpsellPricing = (ICustomUpsellPricing) Activator.CreateInstance(customUpsellPricingType);

        return customUpsellPricing.GetPrice(forexConverter, upsellContent, currency, cartTotal);
    }
}
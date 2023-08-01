using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Content;

public class UpsellContent : UmbracoContent<UpsellContent> {
    public string Description => GetValue(x => x.Description);
    public DonationItem DonationItem => GetAs(x => x.DonationItem);
    public FundDimension1Value Dimension1 => GetAs(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetAs(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetAs(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetAs(x => x.Dimension4);
    public GivingType GivingType => GivingTypes.Donation;
    public decimal FixedAmount => GetValue(x => x.FixedAmount);
    public IEnumerable<PriceHandleElement> PriceHandles => GetNestedAs(x => x.PriceHandles);
    public UpsellPricingMode PricingMode => GetValue(x => x.PricingMode);
    
    public async Task<IAllocation> GetAllocationAsync(decimal amount,
                                                      Currency currency) {
        var fundDimensions = new FundDimensionValues(Dimension1, Dimension2, Dimension3, Dimension4);
        var fundAllocation = new FundAllocation(DonationItem);
        var priceOrAmount = new Money(amount, currency);

        return new Allocation(AllocationTypes.Fund,
                              priceOrAmount,
                              fundDimensions,
                              fundAllocation,
                              null,
                              null,
                              Content().Key);
    }

    public async Task<Money> GetPriceOrAmountInCurrencyAsync(IForexConverter forexConverter,
                                                             IPriceCalculator priceCalculator,
                                                             Currency currency,
                                                             Money regularTotal,
                                                             Money donationTotal) {
        Money priceOrAmount = default;

        if (PricingMode == UpsellPricingModes.DonationItem) {
            var fundDimensions = new FundDimensionValues(Dimension1, Dimension2, Dimension3, Dimension4);
            var price = await priceCalculator.InCurrencyAsync(DonationItem, fundDimensions, currency);

            priceOrAmount = new Money(price.Amount, currency);
        } else if (PricingMode == UpsellPricingModes.FixedAmount) {
            var forexMoney = await forexConverter.BaseToQuote().ToCurrency(currency).ConvertAsync(FixedAmount);

            priceOrAmount = forexMoney.Quote;
        } else if (PricingMode == UpsellPricingModes.Custom) {
            priceOrAmount = GetCustomAmount(regularTotal, donationTotal);
        }

        return priceOrAmount;
    }

    private Money GetCustomAmount(Money regularTotal, Money donationTotal) {
        var customUpsellPricingType = OurAssemblies.GetTypes(t => t.IsConcreteClass() && t.HasParameterlessConstructor()
                                                                                      && t.ImplementsInterface<ICustomUpsellPricing>())
                                                   .Single();

        if (!customUpsellPricingType.HasValue()) {
            return null;
        }

        var getAmountMethod = customUpsellPricingType.GetMethod("GetAmount", new []{typeof(Money), typeof(Money)});

        if (!getAmountMethod.HasValue()) {
            return null;
        }
        
        var customUpsellPricing = Activator.CreateInstance(customUpsellPricingType);
            
        object[] parameters = new object[2];
        parameters[0] = regularTotal;
        parameters[1] = donationTotal;
            
        var customAmount = (Money) getAmountMethod.Invoke(customUpsellPricing, parameters);

        return customAmount;
    }
}
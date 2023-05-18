using N3O.Umbraco.Content;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Content;

public class UpsellContent : UmbracoContent<UpsellContent> {
    public decimal Amount => GetValue(x => x.Amount);
    public string Description => GetValue(x => x.Description);
    public DonationItem DonationItem => GetAs(x => x.DonationItem);
    public FundDimension1Value Dimension1 => GetAs(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetAs(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetAs(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetAs(x => x.Dimension4);
    public GivingType GivingType => GivingTypes.Donation;

    public async Task<IAllocation> GetAllocationAsync(IForexConverter forexConverter,
                                                      IPriceCalculator priceCalculator,
                                                      Currency currency) {
        var fundDimensions = new FundDimensionValues(Dimension1, Dimension2, Dimension3, Dimension4);
        var fundAllocation = new FundAllocation(DonationItem);
        var priceOrAmount = await GetPriceOrAmountInCurrencyAsync(forexConverter, priceCalculator, currency);

        return new Allocation(AllocationTypes.Fund, priceOrAmount, fundDimensions, fundAllocation, null, null, true);
    }

    public async Task<Money> GetPriceOrAmountInCurrencyAsync(IForexConverter forexConverter,
                                                             IPriceCalculator priceCalculator,
                                                             Currency currency) {
        Money priceOrAmount;

        if (DonationItem.HasPricing()) {
            var fundDimensions = new FundDimensionValues(Dimension1, Dimension2, Dimension3, Dimension4);
            var price = await priceCalculator.InCurrencyAsync(DonationItem, fundDimensions, currency);

            priceOrAmount = new Money(price.Amount, currency);
        } else {
            var forexMoney = await forexConverter.BaseToQuote().ToCurrency(currency).ConvertAsync(Amount);

            priceOrAmount = forexMoney.Quote;
        }

        return priceOrAmount;
    }
}
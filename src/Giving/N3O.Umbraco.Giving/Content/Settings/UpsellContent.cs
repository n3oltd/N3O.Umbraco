using N3O.Giving.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Content;

public class UpsellContent : UmbracoContent<UpsellContent> {
    public decimal Amount => GetValue(x => x.Amount);
    public DonationItemsContent DonationItem => GetAs(x => x.DonationItem);
    public FundDimension1Value Dimension1 => GetAs(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetAs(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetAs(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetAs(x => x.Dimension4);
    public GivingType GivingType => GivingTypes.Donation;

    public async Task<IAllocation> GetAllocationAsync(IForexConverter forexConverter, Currency currency) {
        var fundDimensions = new FundDimensionValues(Dimension1, Dimension2, Dimension3, Dimension4);
        var fundAllocation = new FundAllocation(DonationItem.Content().As<DonationItem>());

        var forexAmount = await forexConverter.BaseToQuote()
                                              .ToCurrency(currency)
                                              .ConvertAsync(Amount);
        
        return new Allocation(AllocationTypes.Fund, forexAmount.Quote, fundDimensions, fundAllocation, null, true);
    }
}
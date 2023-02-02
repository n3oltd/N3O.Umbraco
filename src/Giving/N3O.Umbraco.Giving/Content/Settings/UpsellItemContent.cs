using N3O.Giving.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Giving.Content;

public class UpsellItemContent : UmbracoContent<UpsellItemContent> {
    public DonationItemsContent DonationItem => GetAs(x => x.DonationItem);
    public FundDimension1Value Dimension1 => GetAs(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetAs(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetAs(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetAs(x => x.Dimension4);
    public int Amount => GetValue(x => x.Amount);
    
    public IAllocation GetAllocation(Currency currency) {
        var fundDimensions = new FundDimensionValues(Dimension1, Dimension2, Dimension3, Dimension4);
        var fundAllocation = new FundAllocation(DonationItem.Content().As<DonationItem>());
        var value = new Money(Amount, currency);
        
        return  new Allocation(AllocationTypes.Fund, value, fundDimensions, fundAllocation, null, true);
    }

    public GivingType GetGivingType() {
        return GivingTypes.Donation;
    }
}
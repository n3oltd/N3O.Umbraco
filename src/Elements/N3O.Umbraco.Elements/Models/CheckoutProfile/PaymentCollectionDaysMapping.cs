using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class PaymentCollectionDaysMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FlowPaymentMethodCollectionDayOfMonth, PaymentMethodCollectionDayOfMonth>((_, _) => new PaymentMethodCollectionDayOfMonth(), MapMonth);
        mapper.Define<FlowPaymentMethodCollectionDayOfWeek, PaymentMethodCollectionDayOfWeek>((_, _) => new PaymentMethodCollectionDayOfWeek(), MapWeek);
    }

    // Umbraco.Code.MapAll
    private void MapMonth(FlowPaymentMethodCollectionDayOfMonth src,
                          PaymentMethodCollectionDayOfMonth dest,
                          MapperContext ctx) {
        dest.DayOfMonth = new NamedLookupRes();
        dest.DayOfMonth.Id = src.DayOfMonth.Id;
        dest.DayOfMonth.Name = src.DayOfMonth.Name;
        dest.DayOfMonth.Token = src.DayOfMonth.Token;
        
        dest.EarliestChargeDateForNewCredential = src.EarliestChargeDateForNewCredential.ToYearMonthDayString();
    }
    
    private void MapWeek(FlowPaymentMethodCollectionDayOfWeek src,
                         PaymentMethodCollectionDayOfWeek dest,
                         MapperContext ctx) {
        dest.DayOfWeek = new NamedLookupRes();
        dest.DayOfWeek.Id = src.DayOfWeek.Id;
        dest.DayOfWeek.Name = src.DayOfWeek.Name;
        dest.DayOfWeek.Token = src.DayOfWeek.Token;
        
        dest.EarliestChargeDateForNewCredential = src.EarliestChargeDateForNewCredential.ToString();
    }
}
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
    private void MapMonth(FlowPaymentMethodCollectionDayOfMonth src, PaymentMethodCollectionDayOfMonth dest, MapperContext ctx) {
        dest.DayOfMonth = src.DayOfMonth.Name;
        dest.EarliestChargeDateForNewCredential = src.EarliestChargeDateForNewCredential.ToYearMonthDayString();
    }
    
    private void MapWeek(FlowPaymentMethodCollectionDayOfWeek src, PaymentMethodCollectionDayOfWeek dest, MapperContext ctx) {
        dest.DayOfWeek = src.DayOfWeek.Name;
        dest.EarliestChargeDateForNewCredential = src.EarliestChargeDateForNewCredential.ToString();
    }
}
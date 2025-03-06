using NodaTime;

namespace N3O.Umbraco.Elements.Models;

public class FlowPaymentMethodCollectionDayOfMonth {
    public FlowPaymentMethodCollectionDay DayOfMonth { get; set; }
    public LocalDate EarliestChargeDateForNewCredential { get; set; }
}
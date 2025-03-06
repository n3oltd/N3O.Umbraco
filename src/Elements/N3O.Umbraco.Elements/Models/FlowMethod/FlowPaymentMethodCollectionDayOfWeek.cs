using NodaTime;

namespace N3O.Umbraco.Elements.Models;

public class FlowPaymentMethodCollectionDayOfWeek {
    public FlowPaymentMethodCollectionDay DayOfWeek { get; set; }
    public LocalDate EarliestChargeDateForNewCredential { get; set; }
}
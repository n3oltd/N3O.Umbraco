using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.Content;

public interface IPaymentMethodSettings {
    string TransactionDescription { get; }
    string TransactionId { get; }
    IEnumerable<DayOfMonth> RestrictCollectionDaysTo { get; }
}

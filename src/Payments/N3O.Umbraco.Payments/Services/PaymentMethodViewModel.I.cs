using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments;

public interface IPaymentMethodViewModel<in T> where T : PaymentMethod { }

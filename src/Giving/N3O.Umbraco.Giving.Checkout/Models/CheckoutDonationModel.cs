using N3O.Umbraco.Payments.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Checkout.Models;

public class CheckoutDonationModel {
    public CheckoutDonationModel(Entities.Checkout checkout,
                                 IReadOnlyDictionary<PaymentMethod, object> paymentMethods) {
        Checkout = checkout;
        PaymentMethods = paymentMethods;
    }

    public Entities.Checkout Checkout { get; }
    public IReadOnlyDictionary<PaymentMethod, object> PaymentMethods { get; }
}

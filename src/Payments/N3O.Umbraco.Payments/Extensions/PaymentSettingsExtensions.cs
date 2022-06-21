using N3O.Umbraco.Payments.Content;
using N3O.Umbraco.References;
using System;

namespace N3O.Umbraco.Payments.Extensions;

public static class PaymentSettingsExtensions {
    public static string GetTransactionDescription(this IPaymentMethodSettings methodSettings, Reference reference) {
        return Get(methodSettings.TransactionDescription, reference);
    }
    
    public static string GetTransactionId(this IPaymentMethodSettings methodSettings, Reference reference) {
        return Get(methodSettings.TransactionId, reference);
    }
    
    private static string Get(string format, Reference reference) {
        return format.Replace("{Reference}", reference.Text, StringComparison.InvariantCultureIgnoreCase);
    }
}

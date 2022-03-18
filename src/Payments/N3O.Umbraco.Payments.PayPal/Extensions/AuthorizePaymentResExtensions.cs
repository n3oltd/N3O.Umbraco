using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.PayPal.Client.Models;

namespace N3O.Umbraco.Payments.PayPal.Extensions {
    public static class AuthorizePaymentResExtensions {
        public static bool IsAuthorised(this AuthorizePaymentRes transaction) {
            return HasStatus(transaction, "COMPLETED");
        }
        
        public static bool IsDeclined(this AuthorizePaymentRes transaction) {
            return HasStatus(transaction, "DECLINED");
        }
        
        public static bool IsFailed(this AuthorizePaymentRes transaction) {
            return HasStatus(transaction, "FAILED");
        }
        
        private static bool HasStatus(AuthorizePaymentRes transaction, string status) {
            return transaction.Status.EqualsInvariant(status);
        }
    }
}
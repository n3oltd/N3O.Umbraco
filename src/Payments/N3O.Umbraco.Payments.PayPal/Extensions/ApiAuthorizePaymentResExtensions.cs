using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.PayPal.Clients;

namespace N3O.Umbraco.Payments.PayPal.Extensions {
    public static class ApiAuthorizePaymentResExtensions {
        public static bool IsAuthorised(this ApiAuthorizePaymentRes transaction) {
            return HasStatus(transaction, "COMPLETED");
        }
        
        public static bool IsDeclined(this ApiAuthorizePaymentRes transaction) {
            return HasStatus(transaction, "DECLINED");
        }
        
        public static bool IsFailed(this ApiAuthorizePaymentRes transaction) {
            return HasStatus(transaction, "FAILED");
        }
        
        private static bool HasStatus(ApiAuthorizePaymentRes transaction, string status) {
            return transaction.Status.EqualsInvariant(status);
        }
    }
}
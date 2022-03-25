using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Bambora.Client;

namespace N3O.Umbraco.Payments.Bambora.Extensions {
    public static class ApiTransactionResExtensions {
        public static bool IsAuthorised(this IApiPaymentRes payment) {
            return HasStatus(payment, "Approved");
        }
        
        public static bool IsDeclined(this IApiPaymentRes payment) {
            return HasStatus(payment, "DECLINE");
        }
        
        public static bool RequiresThreeDSecure(this ApiPaymentRes payment) {
            return payment.ThreeDSessionData.HasValue();
        }
        
        private static bool HasStatus(IApiPaymentRes payment, string status) {
            return payment.Message.EqualsInvariant(status);
        }
    }
}
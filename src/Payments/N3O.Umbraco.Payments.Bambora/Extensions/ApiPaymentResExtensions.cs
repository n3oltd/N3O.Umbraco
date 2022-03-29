using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Bambora.Client;

namespace N3O.Umbraco.Payments.Bambora.Extensions {
    public static class ApiPaymentResExtensions {
        public static bool IsAuthorised(this IApiPaymentRes payment) {
            return HasMessage(payment, "APPROVED");
        }
        
        public static bool IsDeclined(this IApiPaymentRes payment) {
            return HasMessage(payment, "DECLINE");
        }
        
        public static bool RequiresThreeDSecure(this ApiPaymentRes payment) {
            return payment.ThreeDSessionData.HasValue();
        }
        
        private static bool HasMessage(IApiPaymentRes payment, string message) {
            return payment.Message.EqualsInvariant(message);
        }
    }
}
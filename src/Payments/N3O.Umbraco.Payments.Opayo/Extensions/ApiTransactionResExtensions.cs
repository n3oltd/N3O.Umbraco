using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Opayo.Client;

namespace N3O.Umbraco.Payments.Opayo.Extensions {
    public static class ApiTransactionResExtensions {
        public static bool IsAuthorised(this ApiTransactionRes transaction) {
            return HasStatus(transaction, "Ok");
        }
        
        public static bool IsDeclined(this ApiTransactionRes transaction) {
            return HasStatus(transaction, "NotAuthed");
        }
        
        public static bool RequiresThreeDSecure(this ApiTransactionRes transaction) {
            return HasStatus(transaction, "3DAuth");
        }
        
        private static bool HasStatus(ApiTransactionRes transaction, string status) {
            return transaction.Status.EqualsInvariant(status);
        }
    }
}
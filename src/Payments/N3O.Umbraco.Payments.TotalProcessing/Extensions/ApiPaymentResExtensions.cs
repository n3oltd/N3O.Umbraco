using Payments.TotalProcessing.Clients.Models;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.Payments.TotalProcessing.Extensions;

public static class ApiPaymentResExtensions {
    //https://totalprocessing.docs.oppwa.com/reference/resultCodes
    private static readonly string SuccessfullyProcessedPayments = "/^000.000.|000.100.1|000.[36]|000.400.[1][12]0/";
    private static readonly string ThreeDSecureRejectionPattern = @"/^000\.400\.[1][0-9][1-9]|000\.400\.2/";
    private static readonly string ExternalBankRejectionPattern = @"/^800\.[17]00|800\.800\.[123]/";
    private static readonly string SystemErrorsRejectionPattern = @"/^800\.[56]|999\.|600\.1|800\.800\.[84]/";
    private static readonly string ValidationPattern = @"200\.[123]|100\.[53][07]|800\.900|100\.[69]00\.500";

    public static bool IsAuthorised(this ApiPaymentRes payment) {
        return HasResultCode(payment, SuccessfullyProcessedPayments);
    }

    public static bool IsDeclined(this ApiPaymentRes payment) {
        return HasResultCode(payment, ExternalBankRejectionPattern);
    }
    
    public static bool IsRejected(this ApiPaymentRes payment) {
        return HasResultCode(payment, ThreeDSecureRejectionPattern) ||
               HasResultCode(payment, SystemErrorsRejectionPattern);
    }
    
    public static bool HasError(this ApiPaymentRes transaction) {
        return HasResultCode(transaction, ValidationPattern);
    }

    private static bool HasResultCode(ApiPaymentRes payment, string pattern) {
        var match = Regex.Match(payment.Result.Code, pattern);

        return match.Success;
    }
}

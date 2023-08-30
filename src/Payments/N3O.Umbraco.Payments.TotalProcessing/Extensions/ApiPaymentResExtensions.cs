using Payments.TotalProcessing.Clients.Models;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.Payments.TotalProcessing.Extensions;

public static class ApiPaymentResExtensions {
    //https://totalprocessing.docs.oppwa.com/reference/resultCodes
    private const string SuccessfullyProcessedTransactions = "/^000.000.|000.100.1|000.[36]|000.400.[1][12]0/";
    private const string ThreeDSecureRejectionPattern = "/^000\\.400\\.[1][0-9][1-9]|000\\.400\\.2/";
    private const string ExternalBankRejectionPattern = "/^800\\.[17]00|800\\.800\\.[123]/";
    private const string SystemErrorsRejectionPattern = " /^800\\.[56]|999\\.|600\\.1|800\\.800\\.[84]/";

    public static bool IsAuthorised(this ApiTransactionRes transaction) {
        return HasResultCode(transaction, SuccessfullyProcessedTransactions);
    }

    public static bool IsDeclined(this ApiTransactionRes transaction) {
        return HasResultCode(transaction, ExternalBankRejectionPattern);
    }
    
    public static bool IsRejected(this ApiTransactionRes transaction) {
        return HasResultCode(transaction, ThreeDSecureRejectionPattern) ||
               HasResultCode(transaction, SystemErrorsRejectionPattern);
    }

    private static bool HasResultCode(ApiTransactionRes transaction, string pattern) {
        var match = Regex.Match(transaction.Result.Code, pattern);

        return match.Success;
    }
}

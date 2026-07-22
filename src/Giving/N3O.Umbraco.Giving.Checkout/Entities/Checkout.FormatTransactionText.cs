using N3O.Umbraco.Extensions;
using System;
using System.Globalization;

namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    private string FormatTransactionText(string format, string idempotencyKey = null) {
        return format.Replace("{Reference}", Reference.Text, StringComparison.InvariantCultureIgnoreCase)
                     .Replace("{Prefix}",
                              Progress.CurrentStage.TransactionIdPrefix,
                              StringComparison.InvariantCultureIgnoreCase)
                     .Replace("{IdempotencyKey}",
                              idempotencyKey?.GetDeterministicHashCode(true).ToString(CultureInfo.InvariantCulture),
                              StringComparison.InvariantCultureIgnoreCase);
    }
}

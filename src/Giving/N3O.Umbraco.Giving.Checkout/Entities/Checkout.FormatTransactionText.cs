using System;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        private string FormatTransactionText(string format) {
            var res = format.Replace("{Reference}", Reference.Text, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{Prefix}", Progress.CurrentStage.TransctionIdPrefix, StringComparison.InvariantCultureIgnoreCase);
            return res;
        }
    }
}
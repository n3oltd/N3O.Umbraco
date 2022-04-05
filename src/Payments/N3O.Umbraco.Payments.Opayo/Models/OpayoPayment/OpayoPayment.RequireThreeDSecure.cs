using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void RequireThreeDSecure(string vendorTxCode,
                                        string transactionId,
                                        string returnUrl,
                                        string acsUrl,
                                        string acsTransId,
                                        string cReq,
                                        string paReq) {
            ClearErrors();

            VendorTxCode = vendorTxCode;
            OpayoTransactionId = transactionId;
            ReturnUrl = returnUrl;

            if (cReq.HasValue()) {
                RequireThreeDSecureV2(acsUrl, acsTransId, null, cReq);
            } else if (paReq.HasValue()) {
                RequireThreeDSecureV1(acsUrl, vendorTxCode, paReq);
            } else {
                throw new Exception($"Either {nameof(cReq)} or {nameof(paReq)} must be specified");
            }
        }
    }
}
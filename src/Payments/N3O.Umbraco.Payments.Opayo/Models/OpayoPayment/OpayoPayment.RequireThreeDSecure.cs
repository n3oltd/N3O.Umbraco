using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Models;
using System;

namespace N3O.Umbraco.Payments.Opayo.Models;

public partial class OpayoPayment {
    public void RequireThreeDSecure(string transactionId,
                                    string returnUrl,
                                    string acsUrl,
                                    string acsTransId,
                                    string cReq,
                                    string paReq) {
        ClearErrors();

        OpayoTransactionId = transactionId;
        ReturnUrl = returnUrl;

        if (cReq.HasValue()) {
            RequireThreeDSecureV2(ThreeDSecureV2.FromParameters(acsUrl, acsTransId, null, cReq));
        } else if (paReq.HasValue()) {
            RequireThreeDSecureV1(ThreeDSecureV1.FromParameters(acsUrl, null, paReq));
        } else {
            throw new Exception($"Either {nameof(cReq)} or {nameof(paReq)} must be specified");
        }
    }
}

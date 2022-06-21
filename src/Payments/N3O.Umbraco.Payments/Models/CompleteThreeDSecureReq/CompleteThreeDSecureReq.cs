using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Payments.Models;

public class CompleteThreeDSecureReq {
    [Name("CRes")]
    public string CRes { get; set; }

    [Name("PaRes")]
    public string PaRes { get; set; }

    public int Version() {
        if (PaRes.HasValue()) {
            return 1;
        } else {
            return 2;
        }
    }
}

namespace N3O.Umbraco.Payments.Models;

public class ThreeDSecureV1 : Value {
    public ThreeDSecureV1(string acsUrl, string md, string paReq, string paRes) {
        AcsUrl = acsUrl;
        MD = md;
        PaReq = paReq;
        PaRes = paRes;
    }

    public string AcsUrl { get; }
    public string MD { get; }
    public string PaReq { get; }
    public string PaRes { get; }

    public ThreeDSecureV1 Complete(string paRes) {
        return new ThreeDSecureV1(AcsUrl, MD, PaReq, paRes);
    }

    public static ThreeDSecureV1 FromParameters(string acsUrl, string md, string paReq) {
        return new ThreeDSecureV1(acsUrl, md, paReq, null);
    }
}

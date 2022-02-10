using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class StoreCardReq {
        [Name("Advance Payment")]
        public ChargeCardReq AdvancePayment { get; set; }
    }
}
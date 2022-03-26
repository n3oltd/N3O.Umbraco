using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Payments.Bambora.Models {
    public class ChargeCardReq {
        [Name("Token")]
        public string Token { get; set; }

        [Name("Value")]
        public MoneyReq Value { get; set; }

        [Name("Return URL")]
        public string ReturnUrl { get; set; }
    }
}
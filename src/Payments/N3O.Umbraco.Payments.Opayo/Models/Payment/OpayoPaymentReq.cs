using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoPaymentReq {
        [Name("Card Identifier")]
        public string CardIdentifier { get; set; }

        [Name("Merchant SessionKey")]
        public string MerchantSessionKey { get; set; }

        [Name("Value")]
        public MoneyReq Value { get; set; }
        
        [Name("Save Card")]
        public bool? SaveCard { get; set; }
    }
}
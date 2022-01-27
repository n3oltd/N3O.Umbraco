using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Lookups;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoPaymentReq {
        [Name("Card Identifier")]
        public string CardIdentifier { get; set; }

        [Name("Merchant SessionKey")]
        public string MerchantSessionKey { get; set; }

        [Name("Value")]
        public MoneyReq Value { get; set; }

        [Name("Callback Url")]
        public string CallbackUrl { get; set; }

        [Name("Browser Parameters")]
        public BrowserParametersReq BrowserParameters { get; set; }

        [Name("Challenge Window Size")]
        public ChallengeWindowSize ChallengeWindowSize { get; set; }
    }
}
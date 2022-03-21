using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public class ApiPaymentRes {
        [JsonProperty(PropertyName = "id")]
        public string TransactionId { get; set; }

        [JsonProperty(PropertyName = "approved")]
        public string Approved { get; set; }

        [JsonProperty(PropertyName = "message_id")]
        public string MessageId { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "auth_code")]
        public string AuthCode { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string TransType { get; set; }

        [JsonProperty(PropertyName = "payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty(PropertyName = "card", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public CardResponse Card { get; set; }


        [JsonProperty(PropertyName = "links", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Link> Links { get; set; }
    }
}
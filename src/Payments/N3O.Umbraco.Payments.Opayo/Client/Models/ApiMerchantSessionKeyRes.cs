using N3O.Umbraco.Payments.Opayo.Models;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Extensions;
using System;

namespace N3O.Umbraco.Payments.Opayo.Client {
    public class ApiMerchantSessionKeyRes : IMerchantSessionKey {
        [JsonProperty("merchantSessionKey")]
        public string Key { get; set; }

        [JsonProperty("expiry")]
        public string ExpiresAt { get; set; }
        
        [JsonIgnore]
        Instant IMerchantSessionKey.ExpiresAt => DateTime.Parse(ExpiresAt).ToUniversalTime().ToInstant();
    }
}
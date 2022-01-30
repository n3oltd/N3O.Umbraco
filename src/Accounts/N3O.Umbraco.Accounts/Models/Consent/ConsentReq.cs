using N3O.Umbraco.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models {
    public class ConsentReq : IConsent {
        [Name("Choices")]
        public IEnumerable<ConsentChoiceReq> Choices { get; set; }

        [JsonIgnore]
        IEnumerable<IConsentChoice> IConsent.Choices => Choices;
    }
}
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Accounts.Models {
    public class Consent : Value, IConsent {
        [JsonConstructor]
        public Consent(IEnumerable<ConsentChoice> choices) {
            Choices = choices.OrEmpty().ToList();
        }

        public Consent(IConsent consent)
            : this(consent.Choices.OrEmpty().Select(x => new ConsentChoice(x))) { }

        public IEnumerable<ConsentChoice> Choices { get; }

        [JsonIgnore]
        IEnumerable<IConsentChoice> IConsent.Choices => Choices;
    }
}
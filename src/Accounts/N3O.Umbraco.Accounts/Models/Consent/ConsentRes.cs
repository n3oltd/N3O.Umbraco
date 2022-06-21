using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models;

public class ConsentRes : IConsent {
    public IEnumerable<ConsentChoiceRes> Choices { get; set; }

    [JsonIgnore]
    IEnumerable<IConsentChoice> IConsent.Choices => Choices;
}

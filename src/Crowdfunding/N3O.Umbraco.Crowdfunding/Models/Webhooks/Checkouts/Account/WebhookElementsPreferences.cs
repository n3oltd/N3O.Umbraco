using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookElementsPreferences : Value {
    [JsonConstructor]
    public WebhookElementsPreferences(IEnumerable<WebhookElementsPreferenceSelection> selections) {
        Selections = selections.OrEmpty().ToList();
    }

    public IEnumerable<WebhookElementsPreferenceSelection> Selections { get; }

    public Consent ToConsent(ILookups lookups) {
        var consentChoices = new List<ConsentChoice>();
        
        foreach (var preferenceSelection in Selections) {
            var consentChoice = preferenceSelection.ToConsentChoice(lookups);
            
            consentChoices.Add(consentChoice);
        }
        
        return new Consent(consentChoices);
    }
    
    protected override IEnumerable<object> GetAtomicValues() {
        yield return Selections;
    }
}

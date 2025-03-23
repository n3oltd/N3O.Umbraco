using N3O.Umbraco.Extensions;
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
    
    protected override IEnumerable<object> GetAtomicValues() {
        yield return Selections;
    }
}

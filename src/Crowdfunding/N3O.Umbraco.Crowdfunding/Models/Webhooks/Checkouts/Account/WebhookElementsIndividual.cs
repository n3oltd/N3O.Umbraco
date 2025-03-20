using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookElementsIndividual : Value {
    public WebhookElementsIndividual(WebhookElementsName name) {
        Name = name;
    }

    public WebhookElementsName Name { get; }
    

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Name;
    }
}
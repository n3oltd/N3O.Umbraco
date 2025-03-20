using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookElementsEmail : Value {
    public WebhookElementsEmail(string address) {
        Address = address;
    }

    public string Address { get; }
    
    protected override IEnumerable<object> GetAtomicValues() {
        yield return Address;
    }
}

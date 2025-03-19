using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookElementsEmail : Value {
    public WebhookElementsEmail(string address) {
        Address = address;
    }

    public string Address { get; }
    
    public Accounts.Models.Email ToEmail() {
        var email = new Accounts.Models.Email(Address);
        
        return email;
    }
    
    protected override IEnumerable<object> GetAtomicValues() {
        yield return Address;
    }
}

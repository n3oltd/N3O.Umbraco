using N3O.Umbraco.Accounts.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookElementsName : Value {
    public WebhookElementsName(string title, string firstName, string lastName) {
        Title = title;
        FirstName = firstName;
        LastName = lastName;
    }

    public string Title { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public Name ToName() {
        var name = new Name(Title, FirstName, LastName);
        
        return name;
    }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Title;
        yield return FirstName;
        yield return LastName;
    }
}
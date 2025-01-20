using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Content;

public class ConsentOptionContent : UmbracoContent<ConsentOptionContent> {
    public ConsentChannel Channel => GetValue(x => x.Channel);
    public IEnumerable<ConsentCategory> Categories => GetPickedAs(x => x.Categories);
    public string Statement => GetValue(x => x.Statement);
    public bool OptIn => GetValue(x => x.OptIn);

    public ConsentOption ToConsentOption() {
        return new ConsentOption(Channel, Categories, Statement);
    }
}

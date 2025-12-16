using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Models;
using System;

namespace N3O.Umbraco.Cloud.Lookups;

public class AccountLabel : Label {
    public AccountLabel(string id, string name) : base(id, name) { }
}

[Order(int.MaxValue)]
public class AccountLabels : Labels<AccountLabel> {
    public AccountLabels(ICdnClient cdnClient) : base(cdnClient) { }

    protected override AccountLabel CreateLabel(PublishedTagDefinition publishedTagDefinition) {
        return new AccountLabel(publishedTagDefinition.Id, publishedTagDefinition.Name);
    }

    protected override TagScope Scope => TagScopes.AccountLabel;

    protected override TimeSpan CacheDuration => TimeSpan.FromHours(1);
}
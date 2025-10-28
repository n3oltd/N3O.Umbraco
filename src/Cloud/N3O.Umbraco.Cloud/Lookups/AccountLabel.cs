using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class AccountLabel : NamedLookup {
    public AccountLabel(string id, string name) : base(id, name) { }
}

public class AccountLabels : StaticLookupsCollection<AccountLabel> {
    public static readonly AccountLabel FirstTimeDonor = new("firstTimeDonor", "First Time Donor");
    public static readonly AccountLabel OrphanSponsor = new("orphanSponsor", "Orphan Sponsor");
}
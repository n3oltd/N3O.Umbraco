using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class PageAccess : Lookup {
    public PageAccess(string id, bool requiresSignIn, bool requiresAccount) : base(id) {
        RequiresSignIn = requiresSignIn;
        RequiresAccount = requiresAccount;
    }
    
    public bool RequiresSignIn { get; }
    public bool RequiresAccount { get; }
}

public class PageAccesses : StaticLookupsCollection<PageAccess> {
    public static readonly PageAccess Anonymous = new("anonymous", false, false);
    public static readonly PageAccess SignedIn = new("signedIn", true, false);
    public static readonly PageAccess SignedInWithAccount = new("signedInWithAccount", true, true);
}
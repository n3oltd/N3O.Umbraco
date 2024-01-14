using N3O.Umbraco.Lookups;
using System.Text;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class FundraisingPageMode : NamedLookup {
    public FundraisingPageMode(string id, string name) : base(id, name) { }
}

public class FundraisingPageModes : StaticLookupsCollection<FundraisingPageMode> {
    public static readonly FundraisingPageMode View = new("view", "View");
    public static readonly FundraisingPageMode Edit = new("Edit", "edit");
}

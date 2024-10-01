using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crm.Lookups;

public class CrowdfunderActivationAction : Lookup {
    public CrowdfunderActivationAction(string id, string label) : base(id) {
        Label = label;
    }
    
    public string Label { get; }
}

public class CrowdfunderActivationActions : StaticLookupsCollection<CrowdfunderActivationAction> {
    public static readonly CrowdfunderActivationAction Activate = new("activate", "Activate");
    public static readonly CrowdfunderActivationAction Deactivate = new("deactivate", "Deactivate");
}
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crm.Lookups;

public class CrowdfunderStatus : NamedLookup {
    public CrowdfunderStatus(string id,
                             string name,
                             bool canToggle,
                             CrowdfunderActivationAction toggleAction,
                             bool canEdit)
        : base(id, name) {
        CanToggle = canToggle;
        ToggleAction = toggleAction;
        CanEdit = canEdit;
    }
    
    public bool CanToggle { get; }
    public CrowdfunderActivationAction ToggleAction { get; }
    public bool CanEdit { get; set; }
}

public class CrowdfunderStatuses : StaticLookupsCollection<CrowdfunderStatus> {
    public static readonly CrowdfunderStatus Active = new("active",
                                                          "Active",
                                                          true,
                                                          CrowdfunderActivationActions.Deactivate,
                                                          true);
    
    public static readonly CrowdfunderStatus Draft = new("draft",
                                                         "Draft",
                                                         true,
                                                         CrowdfunderActivationActions.Activate,
                                                         true);
    
    public static readonly CrowdfunderStatus Ended = new("ended",
                                                         "Ended",
                                                         false,
                                                         null,
                                                         false);

    public static readonly CrowdfunderStatus Inactive = new("inactive",
                                                            "Inactive",
                                                            true,
                                                            CrowdfunderActivationActions.Activate,
                                                            true);
}
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Engage.Lookups;

public class CrowdfunderStatus : KeyedNamedLookup {
    public CrowdfunderStatus(string id,
                             string name,
                             uint key,
                             bool canToggle,
                             CrowdfunderActivationAction toggleAction,
                             bool canEdit)
        : base(id, name, key) {
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
                                                          2,
                                                          true,
                                                          CrowdfunderActivationActions.Deactivate,
                                                          true);
    
    public static readonly CrowdfunderStatus Draft = new("draft",
                                                         "Draft",
                                                         1,
                                                         true,
                                                         CrowdfunderActivationActions.Activate,
                                                         true);
    
    public static readonly CrowdfunderStatus Ended = new("ended",
                                                         "Ended",
                                                         4,
                                                         false,
                                                         null,
                                                         false);

    public static readonly CrowdfunderStatus Inactive = new("inactive",
                                                            "Inactive",
                                                            3,
                                                            true,
                                                            CrowdfunderActivationActions.Activate,
                                                            true);
}
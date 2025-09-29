using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DesignationType : NamedLookup {
    public DesignationType(string id, string name, string contentTypeAlias) : base(id, name) {
        ContentTypeAlias = contentTypeAlias;
    }
    
    public string ContentTypeAlias { get; }
}

public class DesignationTypes : StaticLookupsCollection<DesignationType> {
    public static readonly DesignationType Fund = new("fund",
                                                      "Fund",
                                                      PlatformsConstants.Designations.Fund);

    public static readonly DesignationType Feedback = new("feedback",
                                                          "Feedback",
                                                          PlatformsConstants.Designations.Feedback);

    public static readonly DesignationType Sponsorship = new("sponsorship",
                                                             "Sponsorship",
                                                             PlatformsConstants.Designations.Sponsorship);
}
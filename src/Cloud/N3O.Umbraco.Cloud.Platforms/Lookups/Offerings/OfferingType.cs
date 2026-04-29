using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

// TODO Delete me and replace with an extension method to convert the allocation type to the content type in Umbraco
public class OfferingType : NamedLookup {
    public OfferingType(string id, string name, string contentTypeAlias) : base(id, name) {
        ContentTypeAlias = contentTypeAlias;
    }

    public string ContentTypeAlias { get; }
}

public class OfferingTypes : StaticLookupsCollection<OfferingType> {
    public static readonly OfferingType Feedback = new("feedback",
                                                       "Feedback",
                                                       PlatformsConstants.Offerings.Feedback);
    
    public static readonly OfferingType Fund = new("fund",
                                                   "Fund",
                                                   PlatformsConstants.Offerings.Fund);

    public static readonly OfferingType Sponsorship = new("sponsorship",
                                                          "Sponsorship",
                                                          PlatformsConstants.Offerings.Sponsorship);
}
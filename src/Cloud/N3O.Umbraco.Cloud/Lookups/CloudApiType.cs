using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class CloudApiType : Lookup {
    public CloudApiType(string id) : base(id) { }
}

public class CloudApiTypes : StaticLookupsCollection<CloudApiType> {
    public static readonly CloudApiType Connect = new("connect");
    public static readonly CloudApiType Engage = new("engage");
}
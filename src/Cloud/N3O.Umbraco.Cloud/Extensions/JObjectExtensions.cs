using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json.Linq;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class JObjectExtensions {
    public static PublishedFileKind GetPublishedFileKind(this JObject jObject) {
        var kindId = jObject["kind"]?.ToString();
        var kind = StaticLookups.FindById<PublishedFileKind>(kindId);

        return kind;
    }
}

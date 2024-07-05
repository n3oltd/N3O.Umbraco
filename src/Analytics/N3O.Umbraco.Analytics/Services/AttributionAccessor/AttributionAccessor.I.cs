using Newtonsoft.Json.Linq;

namespace N3O.Umbraco.Analytics;

public interface IAttributionAccessor {
    JObject GetAttribution();
}
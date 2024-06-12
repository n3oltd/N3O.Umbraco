using N3O.Umbraco.Analytics.Models;

namespace N3O.Umbraco.Analytics;

public interface IAttributionAccessor {
    Attribution GetAttribution();
}
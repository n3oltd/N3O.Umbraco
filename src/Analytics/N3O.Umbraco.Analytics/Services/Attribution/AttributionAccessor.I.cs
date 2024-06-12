using N3O.Umbraco.Analytics.Models;

namespace N3O.Umbraco.Analytics.Services;

public interface IAttributionAccessor {
    Attribution GetAttribution();
}
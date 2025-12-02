using N3O.Umbraco.Extensions;
using System.Linq;

namespace N3O.Umbraco.Features.DynamicListViews;

public static class ContentPathHelper {
    public static bool DynamicListViewsEnabled(string path) {
        var pathIds = path.Split(',');

        if (pathIds.HasAny(x => x.HasValue())) {
            var ids = DynamicListViewsCache.GetIds();
            
            return ids.ContainsAny(pathIds.Select(int.Parse));
        } else {
            return false;   
        }
    }
}
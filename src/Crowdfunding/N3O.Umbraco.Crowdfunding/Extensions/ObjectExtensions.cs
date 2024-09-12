using N3O.Umbraco.Extensions;
using System.Linq;

namespace N3O.Umbraco.CrowdFunding.Extensions;

public static class ObjectExtensions {
    public static object ResolvePathValue(this object data, string path, char pathSeparator = '.') {
        var pathComponents = path.Split(pathSeparator);

        var obj = data;
        
        foreach (var pathComponent in pathComponents) {
            if (obj == null) {
                break;
            }
            
            var propertyInfo = obj.GetType().GetProperties().SingleOrDefault(x => x.Name.EqualsInvariant(pathComponent));

            obj = propertyInfo?.GetValue(obj);
        }

        return obj;
    }
}
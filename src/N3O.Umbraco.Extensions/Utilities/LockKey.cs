using N3O.Umbraco.Extensions;
using System.Linq;

namespace N3O.Umbraco.Utilities;

public static class LockKey {
    public static string Generate<T>(params object[] values) {
        return (typeof(T).Name + "|" + values.ExceptNull().Select(x => x.ToString()).Join("|")).ToLowerInvariant();
    }
}

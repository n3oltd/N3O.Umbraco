using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Utilities {
    public static class CacheKey {
        public static string Generate<T>(params object[] values) {
            return (typeof(T).Name + "|" + values.Join("|")).ToLowerInvariant();
        }
    }
}
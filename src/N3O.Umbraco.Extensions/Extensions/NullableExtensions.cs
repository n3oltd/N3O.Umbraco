using System;

namespace N3O.Umbraco.Extensions {
    public static class NullableExtensions {
        public static T GetValueOrThrow<T>(this T? item) where T : struct {
            if (item == null) {
                throw new Exception($"null value passed to {nameof(GetValueOrThrow)}");
            }

            return item.Value;
        }
    }
}

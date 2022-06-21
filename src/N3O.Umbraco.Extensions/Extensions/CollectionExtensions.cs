using System.Collections.Generic;

namespace N3O.Umbraco.Extensions;

public static class CollectionExtensions {
    public static void AddIfNotExists<T>(this ICollection<T> collection, T element) {
        if (!collection.Contains(element)) {
            collection.Add(element);
        }
    }

    public static void AddRangeIfNotExists<T>(this ICollection<T> collection, IEnumerable<T> elements) {
        foreach (var element in elements) {
            AddIfNotExists(collection, element);
        }
    }
}

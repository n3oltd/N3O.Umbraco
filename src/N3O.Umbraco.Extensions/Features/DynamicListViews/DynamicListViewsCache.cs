using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Collections;

namespace N3O.Umbraco.Features.DynamicListViews;

public class DynamicListViewsCache {
    private static readonly ConcurrentHashSet<int> CustomListViewIds = [];

    public static void Add(int id) {
        CustomListViewIds.AddIfNotExists(id);
    }

    public static void AddRange(IEnumerable<int> ids) {
        CustomListViewIds.AddRangeIfNotExists(ids);
    }

    public static bool Contains(int id) {
        return CustomListViewIds.Contains(id);
    }

    public static bool ContainsAny(IEnumerable<int> ids) {
        return CustomListViewIds.ContainsAny(ids);
    }

    public static IEnumerable<int> GetIds() {
        return CustomListViewIds;
    }

    public static void Remove(int id) {
        CustomListViewIds.Remove(id);
    }
}
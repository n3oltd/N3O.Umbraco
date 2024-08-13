using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.CrowdFunding.Extensions;

public static class EnumerableExtensions {
    public static IReadOnlyList<TRes> ToReadOnlyList<T, TRes>(this IEnumerable<T> source, Func<T, TRes> toRes) {
        return source.OrEmpty().Select(toRes).ToList();
    }
}
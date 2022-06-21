using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Extensions;

public static class RangeExtensions {
    public static Range<T2> Convert<T1, T2>(this Range<T1> range, Func<T1, T2> convert) {
        var from = convert(range.From);
        var to = convert(range.To);
        
        return new Range<T2>(from, to);
    }
    
    public static Range<T> GetValueOrThrow<T>(this Range<T?> nullableRange) where T : struct {
        var from = nullableRange.From.GetValueOrThrow();
        var to = nullableRange.To.GetValueOrThrow();
        
        return new Range<T>(from, to);
    }
    
    public static bool HasFromValue<T>(this Range<T> range) {
        var result = range.HasValue(x => x.From);

        return result;
    }

    public static bool HasToValue<T>(this Range<T> range) {
        var result = range.HasValue(x => x.To);

        return result;
    }
    
    public static bool HasValue<T>(this Range<T> range) {
        var result = HasFromValue(range) || HasToValue(range);

        return result;
    }
    
    public static IReadOnlyList<int> ToEnumerable(this Range<int> range) {
        return ToEnumerable(range, (x, y) => x < y, x => x + 1).ToList();
    }
    
    public static IReadOnlyList<LocalDate> ToEnumerable(this Range<LocalDate> range) {
        return ToEnumerable(range, (x, y) => x < y, x => x.PlusDays(1)).ToList();
    }
    
    public static IEnumerable<T> ToEnumerable<T>(this Range<T> range,
                                                 Func<T, T, bool> isLessThan,
                                                 Func<T, T> increment) {
        for (var value = range.From; isLessThan(value, range.To); value = increment(value)) {
            yield return value;
        }
    }
}

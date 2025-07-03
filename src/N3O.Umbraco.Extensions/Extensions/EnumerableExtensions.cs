using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Extensions;

public static class EnumerableExtensions {
    private static readonly Random Random = new();

    public static bool AreItemsUnique<T>(this IEnumerable<T> source) {
        if (None(source)) {
            return true;
        }

        return source.Distinct().Count() == source.Count();
    }

    public static decimal? AverageOrDefault<T>(this IEnumerable<T> source, Func<T, decimal> selector) {
        if (None(source)) {
            return null;
        }

        return source.Average(selector);
    }

    public static decimal? AverageOrDefault<T>(this IEnumerable<T> source, Func<T, decimal?> selector) {
        if (None(source)) {
            return null;
        }

        return source.Average(selector);
    }

    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize) {
        while (HasAny(source)) {
            yield return source.Take(batchSize);

            source = source.Skip(batchSize);
        }
    }

    // https://codereview.stackexchange.com/a/184677
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences) {
        if (sequences == null) {
            return null;
        }

        IEnumerable<IEnumerable<T>> emptyProduct = [[]];

        return sequences.Aggregate(emptyProduct,
                                   (acc, seq) => acc.SelectMany(_ => seq,
                                                                (accSeq, item) => accSeq.Concat(new[] { item })));
    }

    public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T item) {
        return source.Concat(item.Yield());
    }

    public static bool Contains(this IEnumerable<string> source, string item, bool caseAndCultureInsensitive) {
        if (caseAndCultureInsensitive) {
            return source.Any(x => string.Equals(x, item, StringComparison.InvariantCultureIgnoreCase));
        }

        return source.Contains(item);
    }

    public static bool ContainsAll(this IEnumerable<string> source,
                                   IEnumerable<string> items,
                                   bool caseAndCultureInsensitive) {
        if (caseAndCultureInsensitive) {
            return items.All(x => source.Any(y => string.Equals(y, x, StringComparison.InvariantCultureIgnoreCase)));
        }

        return items.All(source.Contains);
    }

    public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> items) {
        return items.All(source.Contains);
    }

    public static bool ContainsAll<T>(this IEnumerable<T> source, params T[] items) {
        return items?.All(source.Contains) ?? false;
    }

    public static bool ContainsAll<T>(this IEnumerable<T?> source, IEnumerable<T> items) where T : struct {
        return items.All(i => source.Contains(i));
    }

    public static bool ContainsAny(this IEnumerable<string> source,
                                   IEnumerable<string> items,
                                   bool caseAndCultureInsensitive) {
        if (caseAndCultureInsensitive) {
            return items.Any(x => source.Any(y => string.Equals(y, x, StringComparison.InvariantCultureIgnoreCase)));
        }

        return items.Any(source.Contains);
    }

    public static bool ContainsAny<T>(this IEnumerable<T> source, params T[] items) {
        return Intersects(source, items);
    }

    public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> items) {
        return Intersects(source, items);
    }

    public static bool ContainsExactly<T>(this IEnumerable<T> source, params T[] items) {
        if (source.Count() != items.Length) {
            return false;
        }

        return ContainsAll(source, items);
    }

    public static bool ContainsExactly<T>(this IEnumerable<T> source, IEnumerable<T> items) {
        return ContainsExactly(source, items.ToArray());
    }

    public static void Do<T>(this IEnumerable<T> source, Action<T> action) {
        Do(source, (x, _) => action?.Invoke(x));
    }

    public static void Do<T>(this IEnumerable<T> source, Action<T, int> action) {
        if (None(source)) {
            return;
        }

        var index = 0;

        foreach (var item in source) {
            action?.Invoke(item, index);
        
            index++;
        }
    }

    public static Task DoAsync<T>(this IEnumerable<T> source,
                                  int degreeOfParallelism,
                                  Func<T, Task> action,
                                  IProgress<T> progress = null) {
        return Task.WhenAll(Partitioner.Create(source)
                                       .GetPartitions(degreeOfParallelism)
                                       .Select(partition => Task.Run(async () => {
                                           using (partition) {
                                               while (partition.MoveNext()) {
                                                   await action(partition.Current);

                                                   progress?.Report(partition.Current);
                                               }
                                           }
                                       })));
    }

    public static void DoWithPrevious<TSource>(this IEnumerable<TSource> source,
                                               Action<TSource, TSource> action) {
        using (var iterator = source.GetEnumerator()) {
            if (!iterator.MoveNext()) {
                return;
            }

            var previous = iterator.Current;
            while (iterator.MoveNext()) {
                action(previous, iterator.Current);

                previous = iterator.Current;
            }
        }
    }

    public static bool DoesNotContain<T>(this IEnumerable<T> source, T item) {
        return !source.Contains(item);
    }
    
    public static bool DoesNotContain(this IEnumerable<string> source, string item, bool caseAndCultureInsensitive) {
        return !source.Contains(item, caseAndCultureInsensitive);
    }

    public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T element) {
        return source.Except(element.Yield());
    }

    public static IEnumerable<T> ExceptAt<T>(this IEnumerable<T> source, params int[] indices) {
        return source.Where((_, index) => !indices.Contains(index));
    }

    public static IEnumerable<T> ExceptFirst<T>(this IEnumerable<T> source, int count = 1) {
        return source.ExceptAt(Enumerable.Range(0, count).ToArray());
    }

    public static IEnumerable<T> ExceptLast<T>(this IEnumerable<T> source, int count = 1) {
        var last = source.Count();

        return source.ExceptAt(Enumerable.Range(last - count, count).ToArray());
    }

    public static IEnumerable<T> ExceptNull<T>(this IEnumerable<T> source) {
        return source.Where(i => i != null);
    }

    public static IEnumerable<T> ExceptWhere<T>(this IEnumerable<T> source, Func<T, bool> criteria) {
        return source.Where(i => !criteria(i));
    }

    public static IEnumerable<T> ExceptWhere<T>(this IEnumerable<T> source, params T[] items) {
        if (None(items)) return source;

        return source.Where(x => !items.Contains(x));
    }

    public static T FindInvariant<T>(this IEnumerable<T> collection, Func<T, string> stringPropertyPicker,
                                     string value) {
        return collection.FirstOrDefault(x => stringPropertyPicker(x)?.EqualsInvariant(value) ?? false);
    }
    
    public static T GetClosetItem<T>(this IEnumerable<T> items,
                                     Func<T, int> getDistance,
                                     Func<int, bool> distancePredicate = null,
                                     T defaultValue = default ) {
        var selected = defaultValue;
        var minDistance = int.MaxValue;
        
        foreach (var i in items) {
            var distance = getDistance(i);
            
            if (distance < minDistance && distancePredicate?.Invoke(distance) != false) {
                selected = i;
                minDistance = distance;
            }
        }

        return selected;
    }

    public static IEnumerable<string> GetValues(this NameValueCollection collection) {
        return collection.AllKeys.Select(k => collection[k]);
    }

    public static bool HasAny<TSource>(this IEnumerable<TSource> source) {
        return source?.Any() ?? false;
    }

    public static bool HasAny<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) {
        return source?.Any(predicate) ?? false;
    }

    public static bool HasValue<T>(this IEnumerable<T> source) {
        return source?.Any() ?? false;
    }

    public static int IndexOf<T>(this IEnumerable<T> source, T element) {
        var result = 0;

        foreach (var item in source) {
            if (item == null) {
                if (element == null) {
                    return result;
                }

                continue;
            }

            if (item.Equals(element)) {
                return result;
            }

            result++;
        }

        return -1;
    }

    public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> criteria) {
        var result = 0;

        foreach (var item in source) {
            if (criteria(item)) {
                return result;
            }

            result++;
        }

        return -1;
    }

    public static int IndexOf<T>(this IEnumerable<T> source, T value, IEqualityComparer<T> comparer) {
        comparer ??= EqualityComparer<T>.Default;

        var found = source.Select((item, index) => new {
                              Item = item, Index = index
                          })
                          .FirstOrDefault(x => comparer.Equals(x.Item, value));

        return found?.Index ?? -1;
    }

    public static int IndexOfBy<TSource, TKey>(this IEnumerable<TSource> source,
                                               TKey value,
                                               Func<TSource, TKey> keySelector) {
        return source.Select((x, i) => new {
                         Index = i, Value = x
                     })
                     .FirstOrDefault(x => keySelector(x.Value)?.Equals(value) ?? false)
                     ?.Index ?? -1;
    }

    public static bool Intersects<T>(this IEnumerable<T> source, IEnumerable<T> other) {
        var countSource = source.Count();
        var countOther = other.Count();

        if (countOther < countSource) {
            foreach (var item in other) {
                if (source.Contains(item)) {
                    return true;
                }
            }
        } else {
            foreach (var item in source) {
                if (other.Contains(item)) {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool Intersects<T>(this IEnumerable<T> source, params T[] items) {
        return source.Intersects((IEnumerable<T>) items);
    }

    public static bool IsAnyOf<T>(this T item, params T[] items) {
        return items.Contains(item);
    }

    public static bool IsAnyOf<T>(this T item, IEnumerable<T> items) {
        return IsAnyOf(item, OrEmpty(items).ToArray());
    }

    public static bool IsFirst<T>(this IEnumerable<T> source, T element) {
        return element.Equals(source.First());
    }

    public static bool IsSingle<T>(this IEnumerable<T> source) {
        return IsSingle(source, _ => true);
    }

    public static bool IsSingle<T>(this IEnumerable<T> source, Func<T, bool> criteria) {
        var visitedAny = false;

        foreach (var _ in OrEmpty(source).Where(criteria)) {
            if (visitedAny) {
                return false;
            }

            visitedAny = true;
        }

        return visitedAny;
    }

    public static string Join<T>(this IEnumerable<T> source, string separator) {
        if (source == null || !source.Any()) {
            return null;
        }

        return string.Join(separator, source.Select(x => x.ToString()));
    }

    public static bool SetEquals<T1, T2>(this IEnumerable<T1> source,
                                         IEnumerable<T2> items,
                                         Func<T1, T2, bool> predicate) {
        if (None(source) && !None(items)) {
            return false;
        }

        if (!None(source) && None(items)) {
            return false;
        }

        if (source.Count() != items.Count()) {
            return false;
        }

        foreach (var item in items) {
            if (!IsSingle(source, x => predicate(x, item))) {
                return false;
            }
        }

        return true;
    }

    public static bool Lacks<T>(this IEnumerable<T> source, T item) {
        return !source.Contains(item);
    }

    public static bool LacksAll<T>(this IEnumerable<T> source, IEnumerable<T> items) {
        return !source.ContainsAny(items.ToArray());
    }

    public static bool LacksAny<T>(this IEnumerable<T> source, IEnumerable<T> items) {
        return !source.ContainsAll(items);
    }

    public static R MaxOrDefault<T, R>(this IEnumerable<T> source, Func<T, R> expression) {
        if (None(source)) {
            return default;
        }

        return source.Max(expression);
    }

    public static R? MaxOrNull<T, R>(this IEnumerable<T> source, Func<T, R?> expression) where R : struct {
        if (None(source)) {
            return default;
        }

        return source.Max(expression);
    }

    public static R? MaxOrNull<T, R>(this IEnumerable<T> source, Func<T, R> expression) where R : struct {
        return source.MaxOrNull(item => (R?) expression(item));
    }

    public static R MinOrDefault<T, R>(this IEnumerable<T> source, Func<T, R> expression) {
        if (None(source)) {
            return default;
        }

        return source.Min(expression);
    }

    public static R? MinOrNull<T, R>(this IEnumerable<T> source, Func<T, R?> expression) where R : struct {
        if (None(source)) {
            return default;
        }

        return source.Min(expression);
    }

    public static R? MinOrNull<T, R>(this IEnumerable<T> source, Func<T, R> expression) where R : struct {
        return MinOrNull(source, item => (R?) expression(item));
    }

    public static bool None<T>(this IEnumerable<T> source, Func<T, bool> criteria) {
        return !source.Any(criteria);
    }

    public static bool None<T>(this IEnumerable<T> source) {
        if (source == null) {
            return true;
        }

        return !source.Any();
    }

    public static IEnumerable<T> Or<T>(this IEnumerable<T> collection, IEnumerable<T> other) {
        return HasAny(collection) ? collection : other;
    }

    public static IEnumerable<T> Or<T>(this IEnumerable<T> collection, T item) {
        return HasAny(collection) ? collection : item.Yield();
    }

    public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> collection) {
        return collection ?? [];
    }

    public static T PickRandom<T>(this IEnumerable<T> source, Func<T, bool> predicate = null) {
        var elements = source?.Where(x => predicate?.Invoke(x) ?? true);

        if (elements == null || !elements.Any()) {
            return default;
        }

        var index = Random.Next(0, elements.Count() - 1);

        return elements.ElementAt(index);
    }

    public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, IEnumerable<T> prefix) {
        return prefix.Concat(source);
    }

    public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, params T[] prefix) {
        return prefix.Concat(source);
    }

    public static void Remove<T>(this ICollection<T> collection, IEnumerable<T> itemsToRemove) {
        if (itemsToRemove != null) {
            foreach (var item in itemsToRemove) {
                if (collection.Contains(item)) {
                    collection.Remove(item);
                }
            }
        }
    }
    
    public static async Task<IReadOnlyList<TResult>> SelectListAsync<TSource, TResult>(this IEnumerable<TSource> source,
                                                                                       Func<TSource, Task<TResult>> projection) {
        if (source == null) {
            return null;
        }

        var list = new List<TResult>();

        foreach (var item in source) {
            list.Add(await projection(item));
        }

        return list;
    }

    public static IEnumerable<(T, int)> SelectWithIndex<T>(this IEnumerable<T> source) {
        return source.Select((x, i) => (x, i));
    }

    public static IEnumerable<(TSource Prev, TSource Curr)> SelectWithPrevious<TSource>(this IEnumerable<TSource> source) {
        return SelectWithPrevious(source, (prev, curr) => (prev, curr));
    }

    public static IEnumerable<TResult> SelectWithPrevious<TSource, TResult>(this IEnumerable<TSource> source,
                                                                            Func<TSource, TSource, TResult> projection) {
        using (var iterator = source.GetEnumerator()) {
            TSource previous = default;

            while (iterator.MoveNext()) {
                yield return projection(previous, iterator.Current);

                previous = iterator.Current;
            }
        }
    }

    public static IEnumerable<T> Take<T>(this IEnumerable<T> source, int lowerBound, int upperBound) {
        if (upperBound == 0) {
            return [];
        }

        var result = new List<T>();

        var index = -1;
        foreach (var item in source) {
            index++;

            if (index < lowerBound) {
                continue;
            }

            if (index > upperBound) {
                break;
            }

            result.Add(item);
        }

        return result;
    }

    public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int n) {
        return source.Skip(Math.Max(0, source.Count() - n));
    }

    public static IEnumerable<T> TakePage<T>(this IEnumerable<T> source, int? pageSize, int currentPage) {
        if (pageSize == null) {
            return source;
        }

        if (currentPage < 1) {
            currentPage = 1;
        }

        var startIndex = pageSize.Value * (currentPage - 1);
        var endIndex = startIndex + pageSize.Value;

        if (currentPage > 1 && startIndex > source.Count()) {
            return TakePage(source, pageSize, 1);
        }

        if (currentPage >= 1) {
            endIndex--;
        }

        return Take(source, startIndex, endIndex);
    }

    public static string ToCsv<T>(this IEnumerable<T> source, bool insertSpace = false) {
        return Join(source, insertSpace ? ", " : ",");
    }

    public static Dictionary<string, string> ToDictionary(this NameValueCollection collection) {
        return collection.AllKeys.Where(k => k != null).ToDictionary(k => k, k => collection[k]);
    }

    public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this IEnumerable<KeyValuePair<TKey, TElement>> items) {
        return items.ToDictionary(x => x.Key, x => x.Value);
    }

    public static async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source,
                                                                                                    Func<TSource, Task<TKey>> keySelectorAsync,
                                                                                                    Func<TSource, Task<TElement>> elementSelectorAsync) {
        if (source == null) {
            return null;
        }
        
        var keyValuePairs = await source.SelectListAsync(async x => new KeyValuePair<TKey, TElement>(await keySelectorAsync(x),
                                                                                                     await elementSelectorAsync(x)));

        return keyValuePairs.ToDictionary();
    }
    
    public static IReadOnlyList<U> ToReadOnlyList<T, U>(this IEnumerable<T> source, Func<T, U> map) {
        return source.OrEmpty().Select(map).ToList();
    }
    
    public static async Task<IReadOnlyList<U>> ToReadOnlyListAsync<T, U>(this IEnumerable<T> source, Func<T, Task<U>> map) {
        var tasks = source.OrEmpty().Select(map);
        var results = await Task.WhenAll(tasks);
        
        return results.ToList();
    }

    public static string ToString<T>(this IEnumerable<T> source, string separator) {
        return ToString(source, separator, separator);
    }

    public static string ToString<T>(this IEnumerable<T> source, string separator, string lastSeparator) {
        var result = new StringBuilder();

        var items = source.ToArray();

        for (var i = 0; i < items.Length; i++) {
            var item = items[i];

            if (item != null) {
                result.Append(item);
            }

            if (i < items.Length - 2) {
                result.Append(separator);
            }

            if (i == items.Length - 2) {
                result.Append(lastSeparator);
            }
        }

        return result.ToString();
    }

    public static IEnumerable<string> Trim(this IEnumerable<string> source) {
        if (source == null) {
            return [];
        }

        return source.Where(s => s.HasValue())
                     .Select(s => s.Trim())
                     .Where(s => s.HasValue())
                     .ToList();
    }

    public static IEnumerable<T> Union<T>(this IEnumerable<T> source, params IEnumerable<T>[] others) {
        var result = source;

        foreach (var other in others) {
            result = result.Union(other);
        }

        return result;
    }

    public static IEnumerable<T> Union<T>(this IEnumerable<T> source, params T[] otherItems) {
        return source.Union(otherItems);
    }

    public static T WithMax<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) {
        if (None(source)) {
            return default;
        }

        return source.Aggregate((a, b) => Comparer.Default.Compare(keySelector(a), keySelector(b)) > 0 ? a : b);
    }

    public static T WithMin<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) {
        if (None(source)) return default;

        return source.Aggregate((a, b) => Comparer.Default.Compare(keySelector(a), keySelector(b)) < 0 ? a : b);
    }
}

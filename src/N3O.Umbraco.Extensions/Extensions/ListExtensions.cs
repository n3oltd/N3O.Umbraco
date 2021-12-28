using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Extensions {
    public static class ListExtensions {
        public static void AddRange<T>(this IList<T> source, IEnumerable<T> items) {
            foreach (var item in items) {
                source.Add(item);
            }
        }
    
        public static void RemoveWhere<T>(this IList<T> source, Func<T, bool> selector) {
            lock (source) {
                var itemsToRemove = source.Where(selector);

                foreach (var itemToRemove in itemsToRemove) {
                    source.Remove(itemToRemove);   
                }
            }
        }

        public static void Replace<T>(this IList<T> source, T oldValue, T newValue) {
            while (true) {
                var index = source.IndexOf(oldValue);

                if (index == -1) {
                    break;
                }

                source[index] = newValue;
            }
        }
    }
}

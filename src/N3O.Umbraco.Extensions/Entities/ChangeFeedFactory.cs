using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Entities {
    public class ChangeFeedFactory : IChangeFeedFactory {
        private readonly IServiceProvider _serviceProvider;

        public ChangeFeedFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public IReadOnlyList<IChangeFeed> GetChangeFeeds(Type entityType) {
            var changeFeedType = typeof(IChangeFeed<>).MakeGenericType(entityType);
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(changeFeedType);
            var changeFeeds = (IEnumerable) _serviceProvider.GetService(enumerableType) ?? Enumerable.Empty<IChangeFeed>();
            var list = new List<IChangeFeed>();

            foreach (var changeFeed in changeFeeds) {
                list.Add((IChangeFeed) changeFeed);
            }

            return list;
        }
    }
}
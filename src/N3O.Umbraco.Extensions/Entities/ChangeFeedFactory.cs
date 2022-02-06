using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Entities {
    public class ChangeFeedFactory<T> : IChangeFeedFactory<T> where T : IEntity {
        private readonly Lazy<IEnumerable<IChangeFeed<T>>> _changeFeeds;

        public ChangeFeedFactory(Lazy<IEnumerable<IChangeFeed<T>>> changeFeeds) {
            _changeFeeds = changeFeeds;
        }
        
        public IReadOnlyList<IChangeFeed<T>> GetChangeFeeds() {
            return _changeFeeds.Value.ToList();
        }
    }
}
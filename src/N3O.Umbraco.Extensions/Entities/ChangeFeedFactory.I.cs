using System.Collections.Generic;

namespace N3O.Umbraco.Entities {
    public interface IChangeFeedFactory<T> where T : IEntity {
        IReadOnlyList<IChangeFeed<T>> GetChangeFeeds();
    }
}
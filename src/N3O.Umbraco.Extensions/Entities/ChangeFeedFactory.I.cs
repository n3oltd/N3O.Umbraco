using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Entities;

public interface IChangeFeedFactory {
    IReadOnlyList<IChangeFeed> GetChangeFeeds(Type entityType);
}

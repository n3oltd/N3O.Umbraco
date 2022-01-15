using N3O.Umbraco.Events.Criteria;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Events {
    public interface IEvents {
        IReadOnlyList<T> FindEvents<T>(EventCriteria criteria) where T : IPublishedContent;
    }
}
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content {
    public interface IPublishedContentHelper {
        T GetOrCreateFolder<T>(IPublishedContent content, string name) where T : IPublishedContent;

        void SortChildren<T>(IPublishedContent content, Func<T, object> keySelector)
            where T : IPublishedContent;

        void SortChildrenByName(IPublishedContent content);
    }
}
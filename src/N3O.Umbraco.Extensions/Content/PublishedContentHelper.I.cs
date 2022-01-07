using Newtonsoft.Json.Linq;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content {
    public interface IPublishedContentHelper {
        T GetOrCreateFolder<T>(IPublishedContent content, string name) where T : PublishedContentModel;

        void SortChildren<T>(IPublishedContent content, Func<T, object> keySelector, int userId = 0)
            where T : class, IPublishedContent;

        void SortChildrenByName(IPublishedContent content);

        JObject ToJObject(IPublishedContent content);
    }
}
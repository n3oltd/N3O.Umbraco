using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.StructuredData {
    public abstract class StructuredDataProvider<T> : IStructuredDataProvider where T : IPublishedContent {
        public void AddStructuredData(JsonLd jsonLd, IPublishedContent page) {
            AddStructuredData(jsonLd, (T) page);
        }

        public virtual bool IsProviderFor(IPublishedContent page) {
            return page.GetType().IsAssignableTo(typeof(T));
        }

        protected abstract void AddStructuredData(JsonLd jsonLd, T page);
    }
}

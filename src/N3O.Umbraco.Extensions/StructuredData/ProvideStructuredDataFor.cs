using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.StructuredData;

public abstract class ProvideStructuredDataFor<T> : IProvideStructuredDataFor<T> where T : IPublishedContent {
    public void AddStructuredData(JsonLd jsonLd, IPublishedContent content) {
        AddStructuredData(jsonLd, (T) content);
    }

    protected abstract void AddStructuredData(JsonLd jsonLd, T content);
}

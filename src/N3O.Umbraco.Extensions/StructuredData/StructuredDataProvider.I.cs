using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.StructuredData {
    public interface IStructuredDataProvider {
        void AddStructuredData(JsonLd jsonLd, IPublishedContent page);
        bool IsProviderFor(IPublishedContent page);
    }
}

using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.StructuredData {
    public interface IProvideStructuredDataFor<in TContent> : IProvideStructuredDataFor
        where TContent : IPublishedContent { }

    public interface IProvideStructuredDataFor {
        void AddStructuredData(JsonLd jsonLd, IPublishedContent content);
    }
}

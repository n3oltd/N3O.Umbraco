using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content {
    public interface IContentPublisher {
        bool HasProperty(string propertyTypeAlias);
        void Rename(string name);
        PublishResult SaveAndPublish();

        IContentBuilder Content { get; }
    }
}
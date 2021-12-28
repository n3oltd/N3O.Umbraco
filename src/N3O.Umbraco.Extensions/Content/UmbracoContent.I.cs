using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content {
    public interface IUmbracoContent {
        PublishedContentModel Content { get; set; }
    }
}
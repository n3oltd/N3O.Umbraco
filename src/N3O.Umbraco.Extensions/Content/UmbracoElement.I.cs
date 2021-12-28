using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Element {
    public interface IUmbracoElement {
        PublishedElementModel Content { get; set; }
    }
}
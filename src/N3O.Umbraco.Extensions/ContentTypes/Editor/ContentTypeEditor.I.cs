using N3O.Umbraco.Content;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.ContentTypes;

public interface IContentTypeEditor {
    // Finds a type the way a designer does, by deterministic key and then by alias, so a site that renamed
    // one is not read as not having it. Callers deciding whether a type needs creating resolve it through here
    IContentType Find(string alias);
    IContentTypeDesigner ForExisting(string alias);
    IDocumentTypeDesigner NewDocument(string name, string alias);
    IDocumentTypeDesigner<T> NewDocument<T>() where T : IUmbracoContent;
    IElementTypeDesigner NewElement(string name, string alias);
    IElementTypeDesigner<T> NewElement<T>() where T : IUmbracoElement;
}

using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Content;

public interface IContentProperty {
    IContentType ContentType { get; }
    IPropertyType Type { get; }
    string Alias => Type.Alias;
    string Name => Type.Name;
    object Value { get; }
}

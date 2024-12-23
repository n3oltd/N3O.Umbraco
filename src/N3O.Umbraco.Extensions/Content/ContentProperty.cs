using System.Collections.Generic;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Content;

public class ContentProperty<T> : IContentProperty {
    protected ContentProperty(IContentType contentType, IPropertyType type, T value) {
        ContentType = contentType;
        Type = type;
        Value = value;
    }

    public IContentType ContentType { get; }
    public IPropertyType Type { get; }
    public string Alias => Type.Alias;
    public string Name => Type.Name;
    public T Value { get; }

    object IContentProperty.Value => Value;
}

public class ContentProperty : ContentProperty<object> {
    public ContentProperty(IContentType contentType, IPropertyType type, object value)
        : base(contentType, type, value) { }
}

public class ElementProperty : ContentProperty<IReadOnlyList<ContentProperties>> {
    public ElementProperty(IContentType contentType,
                           IPropertyType type,
                           IReadOnlyList<ContentProperties> value,
                           string json)
        : base(contentType, type, value) {
        Json = json;
    }
    
    public string Json { get; }
}

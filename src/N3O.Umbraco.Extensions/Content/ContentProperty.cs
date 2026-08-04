using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
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

public class ElementsProperty : ContentProperty<IReadOnlyList<ContentProperties>> {
    public ElementsProperty(IContentType contentType,
                            IPropertyType type,
                            IReadOnlyList<ContentProperties> value,
                            IReadOnlyList<ContentProperties> settingsElements,
                            string json)
        : base(contentType, type, value) {
        SettingsElements = settingsElements.OrEmpty().ToList();
        Json = json;
    }

    public string Json { get; }
    public IReadOnlyList<ContentProperties> SettingsElements { get; }
    public IReadOnlyList<ContentProperties> AllElements => Value.OrEmpty().Concat(SettingsElements).ToList();
}

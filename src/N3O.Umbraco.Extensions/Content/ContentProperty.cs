using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Content {
    public class ContentProperty {
        public ContentProperty(IContentType contentType, IPropertyType type, object value) {
            ContentType = contentType;
            Type = type;
            Value = value;
        }

        public IContentType ContentType { get; }
        public IPropertyType Type { get; }
        public string Alias => Type.Alias;
        public string Name => Type.Name;
        public object Value { get; }
    }
}
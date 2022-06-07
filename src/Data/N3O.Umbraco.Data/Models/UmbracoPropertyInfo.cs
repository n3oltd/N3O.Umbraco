using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Models {
    public class UmbracoPropertyInfo {
        public UmbracoPropertyInfo(IContentType contentType,
                                   IPropertyType type,
                                   PropertyGroup group,
                                   IDataType dataType,
                                   IEnumerable<NestedContentInfo> nestedContent) {
            ContentType = contentType;
            Type = type;
            Group = group;
            DataType = dataType;
            NestedContent = nestedContent.OrEmpty().ToList();
        }

        public IContentType ContentType { get; }
        public IPropertyType Type { get; }
        public PropertyGroup Group { get; }
        public IDataType DataType { get; }
        public IReadOnlyList<NestedContentInfo> NestedContent { get; }

        public bool IsNestedContent() => Type.IsNestedContent();
    }
    
    public class NestedContentInfo {
        public NestedContentInfo(IContentType contentType, IEnumerable<UmbracoPropertyInfo> properties) {
            ContentType = contentType;
            Properties = properties.OrEmpty().ToList();
        }

        public IContentType ContentType { get; }
        public IReadOnlyList<UmbracoPropertyInfo> Properties { get; }
    }
}
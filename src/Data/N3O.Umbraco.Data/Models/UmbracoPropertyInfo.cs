using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Models {
    public class UmbracoPropertyInfo {
        public UmbracoPropertyInfo(IContentType contentType,
                                   IPropertyType type,
                                   PropertyGroup group,
                                   IDataType dataType) {
            ContentType = contentType;
            Type = type;
            Group = group;
            DataType = dataType;
        }

        public IContentType ContentType { get; }
        public IPropertyType Type { get; }
        public PropertyGroup Group { get; }
        public IDataType DataType { get; }
    }
}
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.ContentTypes;

public interface IPropertyTypeBuilder {
    // isNew separates what the schema defines from how a site presents it: a property the site already has
    // keeps the label its editors know it by
    void Apply(IPropertyType propertyType, PropertyTypeContext context, bool isNew);
    IDataType ResolveDataType(PropertyTypeContext context);
}

using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Content;

public interface IPropertyBuilder {
    (object, IPropertyType) Build(string propertyAlias, string contentTypeAlias);
}

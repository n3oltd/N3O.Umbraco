using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Models; 

public class DataTypeMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IDataType, DataTypeRes>((_, _) => new DataTypeRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IDataType src, DataTypeRes dest, MapperContext ctx) {
        dest.EditorAlias = src.EditorAlias;
        dest.Name = src.Name;
    }
}
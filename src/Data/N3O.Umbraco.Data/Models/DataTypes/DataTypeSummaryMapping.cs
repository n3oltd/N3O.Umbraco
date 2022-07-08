using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Models; 

public class DataTypeSummaryMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IDataType, DataTypeSummary>((_, _) => new DataTypeSummary(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IDataType src, DataTypeSummary dest, MapperContext ctx) {
        dest.EditorAlias = src.EditorAlias;
        dest.Name = src.Name;
    }
}
using N3O.Umbraco.Data.Entities;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Models;

public class ExportMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<Export, ExportProgressRes>((_, _) => new ExportProgressRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(Export src, ExportProgressRes dest, MapperContext ctx) {
        dest.Id = src.Id;
        dest.IsComplete = src.IsComplete;
        dest.Processed = src.Processed;
    }
}

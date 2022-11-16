using N3O.Umbraco.Data.Entities;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Models;

public class ExportMapping : IMapDefinition {
    private static int Counter;

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<Export, ExportProgressRes>((_, _) => new ExportProgressRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(Export src, ExportProgressRes dest, MapperContext ctx) {
        Counter++;

        dest.Id = src.Id;
        dest.IsComplete = src.IsComplete;

        dest.Text = src.Stage;

        if (src.Stage == DataConstants.Export.Stages.Formatting) {
            dest.Text += $" {src.CollatedRecords} Records";
        }

        dest.Text += ".".Repeat((Counter % 3) + 1);

        if (src.Stage == DataConstants.Export.Stages.Collating && src.CollatedRecords != 0) {
            dest.Text += $" ({src.CollatedRecords} Processed)";
        }
    }
}

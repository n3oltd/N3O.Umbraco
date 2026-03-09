using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Data.Commands;

public class GetExportFileQuery : Request<None, ExportFile> {
    public GetExportFileQuery(ExportId exportId) {
        ExportId = exportId;
    }
    
    public ExportId ExportId { get; }
}

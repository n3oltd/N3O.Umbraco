using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Data.Commands;

public class GetExportProgressQuery : Request<None, ExportProgressRes> {
    public GetExportProgressQuery(ExportId exportId) {
        ExportId = exportId;
    }
    
    public ExportId ExportId { get; }
}

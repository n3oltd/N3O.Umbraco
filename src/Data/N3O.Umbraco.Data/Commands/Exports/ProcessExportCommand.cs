using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Data.Commands;

public class ProcessExportCommand : Request<ExportReq, None> {
    public ProcessExportCommand(ExportId exportId) {
        ExportId = exportId;
    }

    public ExportId ExportId { get; }
}

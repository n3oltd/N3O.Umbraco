using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Data.Commands;

public class CreateExportCommand : Request<ExportReq, ExportProgressRes> {
    public CreateExportCommand(ContainerId containerId, ContentType contentType) {
        ContainerId = containerId;
        ContentType = contentType;
    }
    
    public ContainerId ContainerId { get; }
    public ContentType ContentType { get; }
}

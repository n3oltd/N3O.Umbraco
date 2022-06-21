using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Data.Commands;

public class CreateExportCommand : Request<ExportReq, ExportFile> {
    public CreateExportCommand(ContentId contentId, ContentType contentType) {
        ContentId = contentId;
        ContentType = contentType;
    }
    
    public ContentId ContentId { get; }
    public ContentType ContentType { get; }
}

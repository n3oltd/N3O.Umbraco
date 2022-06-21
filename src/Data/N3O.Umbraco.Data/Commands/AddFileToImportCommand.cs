using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Data.Commands;

public class AddFileToImportCommand : Request<AddFileToImportReq, None> {
    public AddFileToImportCommand(ReferenceId referenceId) {
        ReferenceId = referenceId;
    }

    public ReferenceId ReferenceId { get; }
}

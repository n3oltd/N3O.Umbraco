using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Data.Commands {
    public class ProcessImportCommand : Request<None, None> {
        public ProcessImportCommand(ImportId importId) {
            ImportId = importId;
        }
        
        public ImportId ImportId { get; }
    }
}
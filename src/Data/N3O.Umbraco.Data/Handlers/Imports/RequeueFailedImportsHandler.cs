using N3O.Umbraco.Data.Commands;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Data.Handlers; 

public class RequeueFailedImportsHandler : IRequestHandler<RequeueFailedImportsCommand, None, None> {
    private readonly IImportProcessingQueue _importProcessingQueue;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;

    public RequeueFailedImportsHandler(IImportProcessingQueue importProcessingQueue,
                                       IUmbracoDatabaseFactory umbracoDatabaseFactory) {
        _importProcessingQueue = importProcessingQueue;
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
    }
    
    public Task<None> Handle(RequeueFailedImportsCommand req, CancellationToken cancellationToken) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var failedImports = db.Query<Import>().Where(x => x.Status == ImportStatuses.Error).ToList();
            
            foreach (var failedImport in failedImports) {
                _importProcessingQueue.Add(failedImport);
            }
        }

        return Task.FromResult(None.Empty);
    }
}
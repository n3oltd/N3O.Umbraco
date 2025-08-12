using N3O.Umbraco.Content;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Search.Typesense.Commands;
using N3O.Umbraco.Search.Typesense.NamedParameters;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Search.Typesense.Handlers;

public class IndexContentsOfTypeHandler : IRequestHandler<IndexContentsOfTypeCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly IBackgroundJob _backgroundJob;

    public IndexContentsOfTypeHandler(IContentLocator contentLocator, IBackgroundJob backgroundJob) {
        _contentLocator = contentLocator;
        _backgroundJob = backgroundJob;
    }
    
    public Task<None> Handle(IndexContentsOfTypeCommand req, CancellationToken cancellationToken) {
        var publishedContents = req.ContentType.Run(alias => _contentLocator.All(alias), true);
        
        foreach (var publishedContent in publishedContents) {
            _backgroundJob.EnqueueCommand<IndexContentCommand>(m => m.Add<ContentId>(publishedContent.Key.ToString()));
        }
        
        return Task.FromResult(None.Empty);
    }
}

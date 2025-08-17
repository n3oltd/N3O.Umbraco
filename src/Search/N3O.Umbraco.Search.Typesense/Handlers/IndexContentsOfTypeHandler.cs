using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Search.Typesense.Commands;
using N3O.Umbraco.Search.Typesense.NamedParameters;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Search.Typesense.Handlers;

public class IndexContentsOfTypeHandler : IRequestHandler<IndexContentsOfTypeCommand, None, None> {
    private readonly IContentTypeService _contentTypeService;
    private readonly IContentLocator _contentLocator;
    private readonly IBackgroundJob _backgroundJob;

    public IndexContentsOfTypeHandler(IContentTypeService contentTypeService,
                                      IContentLocator contentLocator,
                                      IBackgroundJob backgroundJob) {
        _contentTypeService = contentTypeService;
        _contentLocator = contentLocator;
        _backgroundJob = backgroundJob;
    }
    
    public Task<None> Handle(IndexContentsOfTypeCommand req, CancellationToken cancellationToken) {
        var contentTypeAlias = req.ContentType.Value;
        var allContentTypes = _contentTypeService.GetAll();

        foreach (var contentType in allContentTypes) {
            if (contentType.Alias.EqualsInvariant(contentTypeAlias) ||
                contentType.CompositionAliases().Contains(contentTypeAlias, true)) {
                var publishedContents = _contentLocator.All(contentType.Alias);
        
                foreach (var publishedContent in publishedContents) {
                    _backgroundJob.EnqueueCommand<IndexContentCommand>(m => m.Add<ContentId>(publishedContent.Key.ToString()));
                }       
            }
        }
        
        return Task.FromResult(None.Empty);
    }
}

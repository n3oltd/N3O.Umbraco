using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Sync.Extensions.Commands;
using N3O.Umbraco.Sync.Extensions.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using uSync.Core;
using uSync.Core.Dependency;
using uSync.Core.Sync;
using uSync.Publisher.Models;
using uSync.Publisher.Process.Models;
using uSync.Publisher.Strategies.Models;
using uSync.Publisher.Strategies.Processor;

namespace N3O.Umbraco.Sync.Extensions.Handlers;

public class SyncContentHandler : IRequestHandler<SyncContentCommand, SyncContentReq, None> {
    private static readonly string Document = global::Umbraco.Cms.Core.Constants.UdiEntityType.Document;

    private readonly IContentLocator _contentLocator;
    private readonly PublisherProcessor _publisherProcessor;

    public SyncContentHandler(IContentLocator contentLocator, PublisherProcessor publisherProcessor) {
        _contentLocator = contentLocator;
        _publisherProcessor = publisherProcessor;
    }

    public async Task<None> Handle(SyncContentCommand req, CancellationToken cancellationToken) {
        var requestId = req.Model.RequestId.GetValueOrThrow();
        var contentId = req.Model.ContentId.GetValueOrThrow();
        var content = _contentLocator.ById(req.Model.ContentId.GetValueOrThrow());

        var syncItem = new SyncItem {
            Udi = Udi.Create(Document, contentId),
            Name = content.Name
        };
        syncItem.Change = ChangeType.Create;
        syncItem.Flags = DependencyFlags.PublishedDependencies;

        var options = new PublisherProcessingOptions();
        options.Mode = PublishMode.Push;
        options.Server = req.Model.ServerAlias;
        options.EntityType = Document;
        options.Items = [syncItem];
        options.PublisherOptions = new SyncPublisherOptions();
        options.PublisherOptions.PublishedDependencies = true;

        var request = new PublisherActionRequest(requestId, requestId.ToString());
        request.Server = req.Model.ServerAlias;
        request.Mode = PublishMode.Push;

        var result = await _publisherProcessor.Process(request, options);

        if (!result.Success) {
            throw new Exception($"Sync of {contentId} failed with error: {result.Message}");
        }

        return None.Empty;
    }
}

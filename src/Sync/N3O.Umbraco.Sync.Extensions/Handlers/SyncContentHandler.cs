﻿using N3O.Umbraco.Content;
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
using uSync.Publisher.Client;
using uSync.Publisher.Models;

namespace N3O.Umbraco.Sync.Extensions.Handlers;

public class SyncContentHandler : IRequestHandler<SyncContentCommand, SyncContentReq, None> {
    private static readonly string Document = global::Umbraco.Cms.Core.Constants.UdiEntityType.Document;
    
    private readonly IPublisherStateService _publisherStateService;
    private readonly IContentLocator _contentLocator;

    public SyncContentHandler(IPublisherStateService publisherStateService, IContentLocator contentLocator) {
        _publisherStateService = publisherStateService;
        _contentLocator = contentLocator;
    }

    public async Task<None> Handle(SyncContentCommand req, CancellationToken cancellationToken) {
        var requestId = req.Model.RequestId.GetValueOrThrow();
        var content = _contentLocator.ById(req.Model.ContentId.GetValueOrThrow());

        if (!_publisherStateService.HasProcess(requestId)) {
            var syncItem = new SyncItem();
            syncItem.Change = ChangeType.Create;
            syncItem.Flags = DependencyFlags.PublishedDependencies;
            syncItem.Udi = Udi.Create(Document, req.Model.ContentId.GetValueOrThrow());
            syncItem.Name = content.Name;

            _publisherStateService.Intiailize(requestId, req.Model.ServerAlias, PublishMode.Push, [syncItem]);
        }
        
        SyncActionState state;
        
        do {
            state = await _publisherStateService.Process(requestId);
        } while (!state.IsComplete);

        if (!state.Sucess) {
            throw new Exception($"Sync of {req.Model.ContentId} failed with error: {state.Message}");
        }
        
        return None.Empty;
    }
}
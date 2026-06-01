using N3O.Umbraco.Content;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Sync.Extensions.Commands;
using N3O.Umbraco.Sync.Extensions.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Sync.Extensions.Handlers;

// TODO: IPublisherStateService (uSync.Publisher.Client namespace) was removed in uSync.Publisher v17.
// The publisher push architecture was rewritten around Jumoo.Processing (a closed-source multi-step
// process library). The old HasProcess / Intiailize / Process / SyncActionState API no longer exists.
// This handler needs to be reimplemented using the replacement API once Jumoo publish the migration
// guide or a public Jumoo.Processing abstraction is available. Until then it throws NotSupportedException
// so it fails loudly at runtime rather than silently doing nothing.
public class SyncContentHandler : IRequestHandler<SyncContentCommand, SyncContentReq, None> {
    private readonly IContentLocator _contentLocator;

    public SyncContentHandler(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }

    public Task<None> Handle(SyncContentCommand req, CancellationToken cancellationToken) {
        throw new NotSupportedException(
            "SyncContentHandler is not supported after the upgrade to uSync.Publisher v17. " +
            "IPublisherStateService was removed. Reimplement using the Jumoo.Processing-based API.");
    }
}
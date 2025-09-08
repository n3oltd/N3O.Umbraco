using N3O.Umbraco.Mediator;
using N3O.Umbraco.Sync.Extensions.Commands;
using N3O.Umbraco.Sync.Extensions.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Sync.Extensions.Handlers;

public class SyncDataHandler : IRequestHandler<SyncDataCommand, SyncDataReq, None> {
    public async Task<None> Handle(SyncDataCommand req, CancellationToken cancellationToken) {
        // TODO resolve the provider from registrations and execute

        throw new NotImplementedException();
    }
}
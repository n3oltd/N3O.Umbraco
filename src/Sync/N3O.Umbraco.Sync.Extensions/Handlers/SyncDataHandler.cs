using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Sync.Extensions.Commands;
using N3O.Umbraco.Sync.Extensions.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Sync.Extensions.Handlers;

public class SyncDataHandler : IRequestHandler<SyncDataCommand, SyncDataReq, None> {
    public async Task<None> Handle(SyncDataCommand req, CancellationToken cancellationToken) {
        var providerRegistration = DataSync.GetRegistrationByProviderId(req.ProviderId);

        if (!providerRegistration.HasValue()) {
            return None.Empty;
        }

        if (providerRegistration.SharedSecret != req.Model.SharedSecret) {
            throw new UnauthorizedAccessException();
        }

        var consumer = (IDataSyncConsumer) Activator.CreateInstance(providerRegistration.ConsumerType);

        await consumer.ConsumeAsync(req.Model.Data);
        
        return None.Empty;
    }
}
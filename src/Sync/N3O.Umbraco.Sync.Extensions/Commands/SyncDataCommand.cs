using N3O.Umbraco.Mediator;
using N3O.Umbraco.Sync.Extensions.Models;
using N3O.Umbraco.Sync.Extensions.NamedParameters;

namespace N3O.Umbraco.Sync.Extensions.Commands;

public class SyncDataCommand : Request<SyncDataReq, None> {
    public SyncDataCommand(ProviderId providerId) {
        ProviderId = providerId;
    }
    
    public ProviderId ProviderId { get; }
}
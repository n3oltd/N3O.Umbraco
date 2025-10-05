using N3O.Umbraco.Json;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Attributes;
using N3O.Umbraco.Sync.Extensions.Commands;
using N3O.Umbraco.Sync.Extensions.Models;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Sync.Extensions.Handlers;

[RecurringJob("Trigger Data Sync", "*/5 * * * *")]
public class TriggerDataSyncHandler : IRequestHandler<TriggerDataSyncCommand, None, None> {
    private readonly IReadOnlyList<SyncRegistration> _syncRegistrations;
    private readonly IJsonProvider _jsonProvider;
    private readonly IUrlBuilder _urlBuilder;

    public TriggerDataSyncHandler(IJsonProvider jsonProvider, IUrlBuilder urlBuilder) {
        _jsonProvider = jsonProvider;
        _urlBuilder = urlBuilder;
        _syncRegistrations = DataSync.GetAllRegistrations().ToList();
    }
    
    public async Task<None> Handle(TriggerDataSyncCommand req, CancellationToken cancellationToken) {
        foreach (var syncRegistration in _syncRegistrations.Where(x => x.IsDue())) {
            var producer = (IDataSyncProducer) Activator.CreateInstance(syncRegistration.ConsumerType);

            var allData = await producer.ProvideAsync();
            
            foreach (var data in allData) {
                var syncDataReq = GetSyncDataReq(syncRegistration.SharedSecret, data);
                
                await SyncDataAsync(syncDataReq, syncRegistration.ProviderId);
            }
            
            syncRegistration.MarkSynchronized();
        }

        return None.Empty;
    }

    private async Task SyncDataAsync(SyncDataReq syncDataReq, string providerId) {
        var baseUrl = _urlBuilder.Root().ToString().TrimEnd('/');
        
        using (var httpClient = new HttpClient()) {
            var reqStr = _jsonProvider.SerializeObject(syncDataReq);
            
            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/umbraco/api/SyncExtensions/{providerId}/syncData");
            request.Content = new StringContent(reqStr, null, "application/json");
            request.Headers.Add("accept", "*/*");

            var response = await httpClient.SendAsync(request);
            
            response.EnsureSuccessStatusCode();
        }
    }

    private SyncDataReq GetSyncDataReq(string sharedSecret, object data) {
        var req = new SyncDataReq();
        req.Data =  data;
        req.SharedSecret = sharedSecret;
        
        return req;
    }
}
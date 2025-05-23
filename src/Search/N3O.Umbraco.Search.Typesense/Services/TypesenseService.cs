using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Search.Typesense.Models;
using Typesense;

namespace N3O.Umbraco.Search.Typesense.Services;

public class TypesenseService : ITypesenseService {
    private readonly ITypesenseClient _client;
    private readonly ILogger<TypesenseService> _logger;
    
    public TypesenseService(ITypesenseClient client,ILogger<TypesenseService> logger) {
        _logger = logger;
        _client = client;
    }


    public async Task CheckConnectionAsync(CancellationToken cancellationToken)
    {
        var health = await _client.RetrieveHealth(cancellationToken);
        _logger.LogInformation($"Health check started : {health.Ok}");
    }
    
    public async Task UpsertAsync( SearchDocument searchDocument, CancellationToken cancellationToken) {
        //_client.UpsertDocument();
    }
}

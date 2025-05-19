using System.Threading;
using System.Threading.Tasks;
using Typesense;

namespace N3O.Umbraco.Search.Typesense;

public interface ITypesenseService {
    Task UpsertAsync(Handlers.SearchDocument searchDocument, CancellationToken cancellationToken);
}

public class TypesenseService : ITypesenseService {
    private readonly TypesenseClient _client;

    public TypesenseService() {
        _client = new global::Typesense.TypesenseClient();
    }
    
    public async Task UpsertAsync(Handlers.SearchDocument searchDocument, CancellationToken cancellationToken) {
        _client.UpsertDocument();
    }
}
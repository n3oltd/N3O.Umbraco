using N3O.Umbraco.Json;
using System.Threading.Tasks;

namespace N3O.Umbraco.Sync.Extensions;

public abstract class DataSyncConsumer<T> : IDataSyncConsumer<T> {
    private readonly IJsonProvider _jsonProvider;

    public DataSyncConsumer(IJsonProvider jsonProvider) {
        _jsonProvider = jsonProvider;
    }
    
    public async Task ConsumeAsync(object content) {
        var typedData = _jsonProvider.DeserializeDynamicTo<T>(content);
        
        await ConsumeAsync(typedData);
    }
    
    public async Task ConsumeAsync(T content) {
        await ProcessAsync(content);
    }
    
    protected abstract Task ProcessAsync(T content);
}
using System.Threading.Tasks;

namespace N3O.Umbraco.Sync.Extensions;

public abstract class DataSyncConsumer<T> : IDataSyncConsumer<T> {
    public async Task ConsumeAsync(object content) {
        await ConsumeAsync((T) content);
    }
    
    public async Task ConsumeAsync(T content) {
        await ProcessAsync(content);
    }
    
    protected abstract Task ProcessAsync(T content);
}
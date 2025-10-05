using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Sync.Extensions;

public abstract class DataSyncProducer<T> : IDataSyncProducer<T> {
    public async Task<IEnumerable<T>> ProvideTypedAsync() {
        return await GetAllDataAsync();
    }

    public async Task<IEnumerable<object>> ProvideAsync() {
        var typedData = await ProvideTypedAsync();
        var data = new  List<object>();
        
        typedData.Do(x => data.Add(x));

        return data;
    }

    protected abstract Task<IEnumerable<T>> GetAllDataAsync();
}
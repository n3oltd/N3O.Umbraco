using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Sync.Extensions;

public interface IDataSyncProducer<T> : IDataSyncProducer {
    Task<IEnumerable<T>> ProvideTypedAsync();
}

public interface IDataSyncProducer {
    Task<IEnumerable<object>> ProvideAsync();
}
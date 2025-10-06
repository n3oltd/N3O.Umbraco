using System.Threading.Tasks;

namespace N3O.Umbraco.Sync.Extensions;

public interface IDataSyncConsumer<T> : IDataSyncConsumer {
    Task ConsumeAsync(T content);
}

public interface IDataSyncConsumer {
    Task ConsumeAsync(object content);
}
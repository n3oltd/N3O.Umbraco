using System.Threading.Tasks;

namespace N3O.Umbraco.Entities;

public interface IChangeFeed {
    Task ProcessChangeAsync(EntityChange entityChange);
}

public interface IChangeFeed<T> : IChangeFeed where T : IEntity { }

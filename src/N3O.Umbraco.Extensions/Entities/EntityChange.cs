namespace N3O.Umbraco.Entities;

public class EntityChange {
    public EntityChange(IEntity sessionEntity, IEntity databaseEntity, EntityOperation operation) {
        SessionEntity = sessionEntity;
        DatabaseEntity = databaseEntity;
        Operation = operation;
    }

    public EntityOperation Operation { get; }
    public IEntity SessionEntity { get; }
    public IEntity DatabaseEntity { get; }
}

public class EntityChange<T> where T : IEntity {
    public EntityChange(T sessionEntity, T databaseEntity, EntityOperation operation) {
        SessionEntity = sessionEntity;
        DatabaseEntity = databaseEntity;
        Operation = operation;
    }

    public EntityOperation Operation { get; }
    public T SessionEntity { get; }
    public T DatabaseEntity { get; }
}

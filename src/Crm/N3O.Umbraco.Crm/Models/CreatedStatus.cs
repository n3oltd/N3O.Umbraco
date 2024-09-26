namespace N3O.Umbraco.Crm.Models;

public class CreatedStatus<T> {
    public CreatedStatus(T entity, bool isCreated) {
        Entity = entity;
        IsCreated = isCreated;
    }

    public T Entity { get; }
    public bool IsCreated { get; }
}

public static class CreatedStatus {
    public static CreatedStatus<T> ForCreated<T>(T entity) => new(entity, true);
    
    public static CreatedStatus<T> ForNotCreated<T>() => new(default, false);
}
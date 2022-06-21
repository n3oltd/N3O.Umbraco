using N3O.Umbraco.Entities;

namespace N3O.Umbraco.References;

public class Counter : Entity {
    public long Next { get; private set; }
    
    public void Increment() {
        Next++;
    }

    public static Counter Create(EntityId id, long startFrom) {
        var counter = Entity.Create<Counter>(id);
        
        counter.Next = startFrom;

        return counter;
    }
}

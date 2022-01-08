using N3O.Umbraco.Entities;

namespace N3O.Umbraco.Counters {
    public class Counter : Entity {
        public long Next { get; private set; }
        
        public void Increment() {
            Next++;
        }

        public void Initialize(long startFrom) {
            Next = startFrom;
        }
    }
}
using N3O.Umbraco.Entities;
using System;

namespace N3O.Umbraco.Counters {
    public class Counter : Entity {
        public long Next { get; private set; }
        
        public void Increment() {
            Next++;
        }

        public static Counter Create(Guid id, long startFrom) {
            var counter = Entity.Create<Counter>(id);
            
            counter.Next = startFrom;

            return counter;
        }
    }
}
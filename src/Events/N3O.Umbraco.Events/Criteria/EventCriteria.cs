using N3O.Umbraco.Events.Content;
using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Events.Criteria {
    public class EventCriteria {
        public Range<LocalDate?> Date { get; set; }
        public IEnumerable<Category> Category { get; set; }
    }
}
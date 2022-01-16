using N3O.Umbraco.Attributes;
using N3O.Umbraco.Events.Content;
using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Events.Criteria {
    public class EventCriteria {
        [Name("Category")]
        public IEnumerable<Category> Category { get; set; }
        
        [Name("Date")]
        public Range<LocalDate?> Date { get; set; }
    }
}
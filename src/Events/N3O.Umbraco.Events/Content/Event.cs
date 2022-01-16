using N3O.Umbraco.Content;
using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Events.Content {
    public class Event : UmbracoContent<Event> {
        public IEnumerable<Category> Categories => GetValue(x => x.Categories);
        public LocalDate Date => GetLocalDate(x => x.Date);
    }
}

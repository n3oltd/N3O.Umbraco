using N3O.Umbraco.Blog.Content;
using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Blog.Criteria {
    public class PostCriteria {
        public Range<LocalDate?> Date { get; set; }
        public IEnumerable<Category> Category { get; set; }
    }
}
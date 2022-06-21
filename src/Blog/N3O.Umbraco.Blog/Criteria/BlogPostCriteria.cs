using N3O.Umbraco.Attributes;
using N3O.Umbraco.Blog.Content;
using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Blog.Criteria;

public class BlogPostCriteria {
    [Name("Category")]
    public IEnumerable<BlogCategory> Category { get; set; }
    
    [Name("Date")]
    public Range<LocalDate?> Date { get; set; }
}

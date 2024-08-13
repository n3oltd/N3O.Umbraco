using N3O.Umbraco.Content;
using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Blog.Content;

public class BlogPostContent : UmbracoContent<BlogPostContent> {
    public IEnumerable<BlogCategory> Categories => GetPickedAs(x => x.Categories);
    public LocalDate Date => GetLocalDate(x => x.Date);
}

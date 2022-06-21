using N3O.Umbraco.Content;
using NodaTime;

namespace N3O.Umbraco.Blog.Content;

public class BlogPostsContent : UmbracoContent<BlogPostsContent> {
    public LocalDate? Date => GetLocalDate(x => x.Date);
}

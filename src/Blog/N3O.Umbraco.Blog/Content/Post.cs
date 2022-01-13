using N3O.Umbraco.Content;
using NodaTime;

namespace N3O.Umbraco.Blog.Content {
    public class Post : UmbracoContent<Post> {
        public Category Category => GetValue(x => x.Category);
        public LocalDate Date => GetValue(x => x.Date);
    }
}

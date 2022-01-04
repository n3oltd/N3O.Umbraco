using N3O.Umbraco.Content;
using NodaTime;

namespace N3O.Umbraco.Blog.Content {
    public class Post : UmbracoContent {
        public Category Category => GetValue<Post, Category>(x => x.Category);
        public LocalDate Date => GetValue<Post, LocalDate>(x => x.Date);
    }
}

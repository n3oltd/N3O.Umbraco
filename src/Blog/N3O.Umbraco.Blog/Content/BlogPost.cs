using N3O.Umbraco.Content;
using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Blog.Content {
    public class BlogPost : UmbracoContent<BlogPost> {
        public IEnumerable<BlogCategory> Categories => GetValue(x => x.Categories);
        public LocalDate Date => GetLocalDate(x => x.Date);
    }
}

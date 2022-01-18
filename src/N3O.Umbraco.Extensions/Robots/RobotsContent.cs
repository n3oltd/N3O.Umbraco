using N3O.Umbraco.Content;

namespace N3O.Umbraco.Robots {
    public class RobotsContent : UmbracoContent<RobotsContent> {
        public string CustomDirectives => GetValue(x => x.CustomDirectives);
    }
}
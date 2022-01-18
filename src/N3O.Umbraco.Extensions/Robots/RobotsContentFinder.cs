using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Robots {
    public class RobotsContentFinder : ContentFinderBase {
        private static readonly string RobotsAlias = AliasHelper<RobotsContent>.ContentTypeAlias();

        public RobotsContentFinder(IContentCache contentCache) : base(contentCache) { }
        
        public override bool TryFindContentImpl(IPublishedRequestBuilder request) {
            var path = GetRequestedPath(request.Uri);

            if (path.EqualsInvariant(RobotsTxt.File)) {
                request.SetPublishedContent(ContentCache.Single(RobotsAlias));
                return true;
            }

            return false;
        }
    }
}
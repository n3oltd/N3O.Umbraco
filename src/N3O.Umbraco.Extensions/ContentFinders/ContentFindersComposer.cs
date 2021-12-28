using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace N3O.Umbraco.ContentFinders {
    public class ContentFindersComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.SetContentLastChanceFinder<Site404ContentFinder>();

            RegisterAll(t => t.ImplementsInterface<IContentFinder>() &&
                             !t.ImplementsInterface<IContentLastChanceFinder>(),
                        (type, index) => builder.ContentFinders().Insert(index, type));
        }
    }
}

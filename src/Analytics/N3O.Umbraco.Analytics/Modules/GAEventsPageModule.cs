using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Analytics.Modules {
    public class GAEventsPageModule : IPageModule {
        private readonly IGAEventsBuilder _gaEventsBuilder;
        private readonly IEnumerable<IGAEventsProvider> _allProviders;

        public GAEventsPageModule(IGAEventsBuilder gaEventsBuilder,
                                  IEnumerable<IGAEventsProvider> allProviders) {
            _gaEventsBuilder = gaEventsBuilder;
            _allProviders = allProviders;
        }

        public bool ShouldExecute(IPublishedContent page) => true;

        public async Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
            var providers = _allProviders.OrEmpty().Where(x => x.IsProviderFor(page)).ToList();

            var gTag = new GTag();

            foreach (var provider in providers) {
                await provider.RunAsync(page, gTag, cancellationToken);
            }

            var javaScript = _gaEventsBuilder.BuildJavaScript(gTag);

            return new Code(javaScript);
        }

        public string Key => AnalyticsConstants.PageModuleKeys.GAEvents;
    }
}

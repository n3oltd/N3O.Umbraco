using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Analytics.Modules {
    public class DataLayerPageModule : IPageModule {
        private readonly Lazy<IDataLayerBuilder> _dataLayerBuilder;
        private readonly IEnumerable<IDataLayerProvider> _allProviders;

        public DataLayerPageModule(Lazy<IDataLayerBuilder> dataLayerBuilder,
                                   IEnumerable<IDataLayerProvider> allProviders) {
            _dataLayerBuilder = dataLayerBuilder;
            _allProviders = allProviders;
        }

        public bool ShouldExecute(IPublishedContent page) => true;

        public async Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
            var providers = _allProviders.OrEmpty().Where(x => x.IsProviderFor(page)).ToList();

            var toPush = new List<object>();

            foreach (var provider in providers) {
                toPush.AddRange(await provider.GetAsync(page, cancellationToken));
            }

            var javaScript = _dataLayerBuilder.Value.BuildJavaScript(toPush);

            return new DataLayerCode(javaScript);
        }

        public string Key => AnalyticsConstants.PageModuleKeys.DataLayer;
    }
}

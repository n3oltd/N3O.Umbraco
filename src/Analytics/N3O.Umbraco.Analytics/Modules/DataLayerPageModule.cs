using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Analytics.Modules {
    public class DataLayerPageModule : IPageModule {
        private readonly IDataLayerBuilder _dataLayerBuilder;
        private readonly IReadOnlyList<IDataLayerProvider> _allProviders;

        public DataLayerPageModule(IDataLayerBuilder dataLayerBuilder, IEnumerable<IDataLayerProvider> allProviders) {
            _dataLayerBuilder = dataLayerBuilder;
            _allProviders = allProviders.OrEmpty().ToList();
        }
    
        public async Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
            var providers = _allProviders.Where(x => x.IsProviderFor(page));

            var toPush = new List<object>();
        
        
            foreach (var provider in providers) {
                toPush.AddRange(await provider.GetAsync(page, cancellationToken));
            }

            var javaScript = _dataLayerBuilder.BuildJavaScript(toPush);

            return new DataLayerCode(javaScript);
        }

        public string Key => AnalyticsConstants.Keys.DataLayer;
    }
}

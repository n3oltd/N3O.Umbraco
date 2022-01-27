using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Forex {
    public class ForexComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddSingleton<IExchangeRateCache, ExchangeRateCache>();
            builder.Services.AddSingleton<IForexConverter, ForexConverter>();
        }
    }
}

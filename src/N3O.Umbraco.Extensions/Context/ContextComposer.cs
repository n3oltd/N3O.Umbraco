using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Context {
    public class ContextComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddSingleton<IBaseCurrencyAccessor, BaseCurrencyAccessor>();
            builder.Services.AddSingleton<ICookieAccessor, CookieAccessor>();
            builder.Services.AddSingleton<ICurrencyAccessor, CurrencyAccessor>();
            builder.Services.AddSingleton<ICurrencyCodeAccessor, CurrencyCodeAccessor>();
            builder.Services.AddSingleton<ICurrentUrlAccessor, CurrentUrlAccessor>();
            builder.Services.AddSingleton<IQueryStringAccessor, QueryStringAccessor>();
            builder.Services.TryAddSingleton<IRemoteIpAddressAccessor, RemoteIpAddressAccessor>();
            builder.Services.TryAddSingleton<IBrowserInfoAccessor, BrowserInfoAccessor>();
        }
    }
}
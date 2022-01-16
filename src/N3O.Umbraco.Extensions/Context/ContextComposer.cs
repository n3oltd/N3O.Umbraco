using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Context {
    public class ContextComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddSingleton<IBaseCurrencyAccessor, BaseCurrencyAccessor>();
            builder.Services.AddScoped<ICookieAccessor, CookieAccessor>();
            builder.Services.AddScoped<ICurrencyAccessor, CurrencyAccessor>();
            builder.Services.AddScoped<ICurrencyCodeAccessor, CurrencyCodeAccessor>();
            builder.Services.AddScoped<ICurrentUrlAccessor, CurrentUrlAccessor>();
            builder.Services.AddScoped<IQueryStringAccessor, QueryStringAccessor>();
            builder.Services.TryAddScoped<IRemoteIpAddressAccessor, RemoteIpAddressAccessor>();
        }
    }
}
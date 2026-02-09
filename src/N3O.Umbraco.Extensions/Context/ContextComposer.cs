using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Context;

public class ContextComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IBaseCurrencyAccessor, BaseCurrencyAccessor>();
        builder.Services.AddSingleton<IBrowserInfoAccessor, BrowserInfoAccessor>();
        builder.Services.AddScoped<ICurrencyAccessor, CurrencyAccessor>();
        builder.Services.AddScoped<ICurrencyCodeAccessor, CurrencyCodeAccessor>();
        builder.Services.AddScoped<IDefaultCurrencyProvider, LookupsDefaultCurrencyProvider>();
        builder.Services.AddSingleton<ICurrentUrlAccessor, CurrentUrlAccessor>();
        builder.Services.AddSingleton<IQueryStringAccessor, QueryStringAccessor>();
        builder.Services.TryAddSingleton<IRemoteIpAddressAccessor, RemoteIpAddressAccessor>();
        builder.Services.TryAddSingleton<IRequestHostAccessor, RequestHostAccessor>();

        RegisterCookies<ICookie>(builder);
        RegisterCookies<IReadOnlyCookie>(builder);
    }

    private void RegisterCookies<T>(IUmbracoBuilder builder) where T : IReadOnlyCookie {
        RegisterAll(t => t.ImplementsInterface<T>(), t => {
            builder.Services.AddScoped(t, t);
            
            builder.Services.AddScoped(typeof(T), serviceProvider => {
                var cookie = (T) serviceProvider.GetRequiredService(t);

                return cookie;
            });
        });
    }
}

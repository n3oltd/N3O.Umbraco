using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Context;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Cdn.Cloudflare {
    public class CloudflareCdnComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddScoped<IRemoteIpAddressAccessor, CloudflareIpAddressAccessor>();
        }
    }
}
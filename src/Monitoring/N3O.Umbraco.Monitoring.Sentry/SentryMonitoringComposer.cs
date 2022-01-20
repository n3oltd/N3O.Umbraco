using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Monitoring.Sentry {
    public class SentryMonitoringComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            if (WebHostEnvironment.IsProduction()) {
                builder.Services.Configure<UmbracoPipelineOptions>(opt => {
                    var filter = new UmbracoPipelineFilter("SentryMonitoring");
                    filter.Endpoints = app => app.UseSentryTracing();

                    opt.AddFilter(filter);
                });
            }
        }
    }
}
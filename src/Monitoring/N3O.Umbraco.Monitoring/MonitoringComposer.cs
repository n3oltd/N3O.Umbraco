using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Logging;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;

namespace N3O.Umbraco.Monitoring;

public class MonitoringComposer :Composer {
    public override void Compose(IUmbracoBuilder builder) {
        RegisterAll(t => t.Implements<ILogEnricher>(),
                    t => builder.Services.AddTransient(typeof(ILogEnricher), t));
    }
}
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.References;

public class CountersComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<ICounters, Counters>();

        builder.Services.AddSingleton<IReferenceStartProvider,DefaultReferenceStartProvider>();
    }
}

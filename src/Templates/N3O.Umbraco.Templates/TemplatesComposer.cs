using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Templates;

[ComposeAfter(typeof(MediaComposer))]
public class TemplatesComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddScoped<IMerger, Merger>();
        builder.Services.AddSingleton<IMediaUrl, TemplatesMediaUrl>();
        
        RegisterAll(t => t.ImplementsInterface<IMergeModelsProvider>(),
                    t => builder.Services.AddTransient(typeof(IMergeModelsProvider), t));
        
        RegisterAll(t => t.ImplementsInterface<ITemplateFormatter>(),
                    t => builder.Services.AddTransient(typeof(ITemplateFormatter), t));
    }
}

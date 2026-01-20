using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Templates;

public class TemplatesComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddScoped<IMerger, Merger>();
        
        RegisterAll(t => t.ImplementsInterface<IMergeModelsProvider>(),
                    t => builder.Services.AddTransient(typeof(IMergeModelsProvider), t));
        
        RegisterAll(t => t.ImplementsInterface<ITemplateFormatter>(),
                    t => builder.Services.AddTransient(typeof(ITemplateFormatter), t));
    }
}

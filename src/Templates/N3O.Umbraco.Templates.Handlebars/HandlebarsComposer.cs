using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Templates.Handlebars.BlockHelpers;
using N3O.Umbraco.Templates.Handlebars.Helpers;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Templates.Handlebars;

public class HandlebarsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IHandlebarsCompiler, HandlebarsCompiler>();
        builder.Services.AddTransient<IHandlebarsFactory, HandlebarsFactory>();
        builder.Services.AddTransient<ITemplateEngine, HandlebarsEngine>();
        
        RegisterAll(t => t.ImplementsInterface<IBlockHelper>(),
                    t => builder.Services.AddTransient(typeof(IBlockHelper), t));
        
        RegisterAll(t => t.ImplementsInterface<IHelper>(),
                    t => builder.Services.AddTransient(typeof(IHelper), t));
    }
}

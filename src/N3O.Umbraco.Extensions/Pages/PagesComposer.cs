using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages;

public class PagesComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<IPageExtension>(),
                    t => builder.Services.AddTransient(typeof(IPageExtension), t));
        
        RegisterAll(t => t.IsSubclassOfType(typeof(PublishedContentModel)),
                    t => RegisterDefaultViewModel(builder, t));

        builder.Services.AddTransient<IPagePipeline, PagePipeline>();
    }
    
    private void RegisterDefaultViewModel(IUmbracoBuilder builder, Type pageType) {
        builder.AddDefaultPageViewModel(pageType);
    }
}

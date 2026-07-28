using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Blocks;

public class BlocksComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsInterface<IBlockModule>(),
                    t => builder.Services.AddTransient(typeof(IBlockModule), t));
        
        RegisterAll(t => t.ImplementsInterface<IBlocksRendererPostProcessor>(),
                    t => builder.Services.AddTransient(typeof(IBlocksRendererPostProcessor), t));

        builder.Services.AddTransient<IBlockPipeline, BlockPipeline>();
        builder.Services.AddTransient<IBlocksRenderer, UmbracoBlocksRenderer>();
        
        ConfigureRazorTemplating(builder);
    }

    // Block preview renders site-supplied Razor partials at runtime, so it depends on ASP.NET runtime Razor
    // compilation being enabled on the host. We deliberately do NOT enable it here: calling
    // AddMvcCore().AddRazorRuntimeCompilation() would create a second MVC builder alongside Umbraco's own
    // AddControllersWithViews() and conflict at startup. N3O.Umbraco.Cms enables it once for the host
    // (CmsStartup: AddControllersWithViews().AddRazorRuntimeCompilation()) and every N3O site installs Cms;
    // the file-provider registration below only takes effect once the host has enabled it.
    private void ConfigureRazorTemplating(IUmbracoBuilder builder) {
        builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options => {
            options.FileProviders.Add(new PhysicalFileProvider(WebHostEnvironment.ContentRootPath));
        });
        
        builder.Services.AddRazorTemplating();
    }
}

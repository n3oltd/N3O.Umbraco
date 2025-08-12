using Microsoft.AspNetCore.Hosting;
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

        builder.Services.AddTransient<IBlockPipeline, BlockPipeline>();
        builder.Services.AddTransient<IBlocksRenderer, UmbracoBlocksRenderer>();
        
        ConfigureRazorPhysicalFileProvider(builder);
    }

    private void ConfigureRazorPhysicalFileProvider(IUmbracoBuilder builder) {
        var webhostEnvironment = builder.Services.BuildServiceProvider().GetRequiredService<IWebHostEnvironment>();
        
        builder.Services.AddMvcCore().AddRazorRuntimeCompilation();
        
        builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
        {
            options.FileProviders.Add(new PhysicalFileProvider(webhostEnvironment.ContentRootPath));
        });
        
        builder.Services.AddRazorTemplating();
    }
}

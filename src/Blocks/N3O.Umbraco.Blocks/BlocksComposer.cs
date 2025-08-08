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

        builder.Services.AddTransient<IBlocksRenderer, UmbracoBlocksRenderer>();

        builder.Services.AddTransient<IBlockPipeline, BlockPipeline>();
        
        builder.Services.AddMvcCore().AddRazorRuntimeCompilation();
        
        builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
        {
            options.FileProviders.Add(new PhysicalFileProvider(@"D:\Development\MuslimHands.Website\src\MuslimHands.Web\"));
        });
        
        builder.Services.AddRazorTemplating();
    }
}

using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.ImageProcessing;

public class ImageProcessingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<IImageBuilder, ImageBuilder>();
        builder.Services.AddTransient<IImagePublisher, ImagePublisher>();
        
        RegisterAll(t => t.ImplementsInterface<IImageOperation>(),
                    t => builder.Services.AddTransient(typeof(IImageOperation), t));
    }
}
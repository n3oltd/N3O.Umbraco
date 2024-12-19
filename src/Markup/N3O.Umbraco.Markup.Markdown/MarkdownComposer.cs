using Markdig;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Markup.Markdown.DataTypes;
using System.Linq;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace N3O.Umbraco.Markup.Markdown;

public class MarkdownComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.PropertyValueConverters().Replace<MarkdownEditorValueConverter, OurMarkdownValueConverter>();
        
        builder.Services.AddSingleton<IMarkupEngine, MarkdownEngine>();

        RegisterAll(t => t.ImplementsInterface<IMarkdownExtension>(),
                    t => builder.Services.AddSingleton(typeof(IMarkdownExtension), t));
        
        builder.Services.AddSingleton<MarkdownPipeline>(serviceProvider => {
            var extensions = serviceProvider.GetServices<IMarkdownExtension>().ApplyAttributeOrdering().ToList();

            var pipelineBuilder = new MarkdownPipelineBuilder();

            extensions.Do(pipelineBuilder.Extensions.Add);
            pipelineBuilder.UsePipeTables();

            return pipelineBuilder.Build();
        });
    }
}

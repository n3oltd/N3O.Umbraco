using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Extensions;
using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public abstract class BlocksRenderer<T> : IBlocksRenderer where T : class {
    private readonly IReadOnlyList<IBlocksRendererPostProcessor> _postProcessors;

    protected BlocksRenderer(IEnumerable<IBlocksRendererPostProcessor> postProcessors) {
        _postProcessors = postProcessors.ApplyAttributeOrdering();
    }
    
    public abstract Task<HtmlString> RenderBlocksAsync(IPublishedContent content, string propertyName);

    protected T GetPropertyAs(IPublishedContent content, string propertyName) {
        var property = content.GetProperty(propertyName);

        if (property == null) {
            throw new Exception($"No property found with name {propertyName.Quote()}");
        }

        var blocks = property.GetValue() as T;
        
        if (blocks == null) {
            throw new Exception($"Property {propertyName.Quote()} does not contain blocks");
        }

        return blocks;
    }
    
    protected async Task<string> RenderViewAsync(IPublishedContent content, string viewPath, object viewModel) {
        var html = await RazorTemplateEngine.RenderPartialAsync(viewPath, viewModel);

        foreach (var postProcessor in _postProcessors) {
            html = await postProcessor.ProcessAsync(content, html);
        }

        return html;
    }
}
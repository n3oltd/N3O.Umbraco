using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Extensions;
using Razor.Templating.Core;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public abstract class BlocksRenderer<T> : IBlocksRenderer where T : class {
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
    
    protected async Task<string> RenderBlockAsync(string viewPath, object viewModel) {
        return await RazorTemplateEngine.RenderPartialAsync(viewPath, viewModel);
    }
}
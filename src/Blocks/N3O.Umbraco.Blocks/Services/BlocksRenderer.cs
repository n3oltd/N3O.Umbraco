using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Extensions;
using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public abstract class BlocksRenderer<T> : IBlocksRenderer where T : class {
    public async Task<HtmlString> RenderBlocksAsync(IPublishedContent content, string propertyName) {
        var property = GetPropertyAs(content, propertyName);
        
        var viewModels = GetViewModels(property);
        var viewPaths = GetViewPaths(property);

        if (viewModels.Count() != viewPaths.Count()) {
            throw new Exception($"{nameof(GetViewModels)} and {nameof(GetViewPaths)} returned different numbers of items");
        }

        var html = new StringBuilder();

        foreach (var (viewModel, index) in viewModels.SelectWithIndex()) {
            var viewPath = viewPaths.ElementAt(index);
            
            var blockHtml = await RenderBlockAsync(viewPath, viewModel);
            
            html.Append(blockHtml);
        } 

        return new HtmlString(html.ToString());
    }

    protected abstract IEnumerable<IBlockViewModel> GetViewModels(T property);
    protected abstract IEnumerable<string> GetViewPaths(T property);

    private T GetPropertyAs(IPublishedContent content, string propertyName) {
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
    
    private async Task<string> RenderBlockAsync(string viewPath, IBlockViewModel viewModel) {
        return await RazorTemplateEngine.RenderPartialAsync(viewPath, viewModel);
    }
}
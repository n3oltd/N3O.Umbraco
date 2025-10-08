﻿using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Extensions;
using Perplex.ContentBlocks.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks.Perplex;

public class PerplexBlocksRenderer : BlocksRenderer<ContentBlocks> {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PerplexBlocksRenderer(IEnumerable<IBlocksRendererPostProcessor> postProcessors,
                                 IHttpContextAccessor httpContextAccessor)
        : base(postProcessors) {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public override async Task<HtmlString> RenderBlocksAsync(IPublishedContent content, string propertyName) {
        var property = GetPropertyAs(content, propertyName);
        
        var viewModels = GetViewModels(property).ToList();
        var viewPaths = GetViewPaths(property);

        if (viewModels.Count() != viewPaths.Count()) {
            throw new Exception($"{nameof(GetViewModels)} and {nameof(GetViewPaths)} returned different numbers of items");
        }

        var html = new StringBuilder();

        foreach (var (viewModel, index) in viewModels.SelectWithIndex()) {
            var viewPath = viewPaths.ElementAt(index);
            
            var blockHtml = await RenderViewAsync(content, viewPath, viewModel);
            
            html.Append(blockHtml);
        } 

        return new HtmlString(html.ToString());
    }

    private IEnumerable<IBlockViewModel> GetViewModels(ContentBlocks contentBlocks) {
        foreach (var contentBlockModel in contentBlocks.Blocks) {
            var factory = new PerplexBlockViewModelFactory<IPublishedElement, PerplexBlockViewModel<IPublishedElement>>(_httpContextAccessor,
                                                                                                                        (_, p) => new PerplexBlockViewModel<IPublishedElement>(p));
        
            var blockModel = (IPerplexBlockViewModel) factory.Create(contentBlockModel.Content,
                                                                     contentBlockModel.Id,
                                                                     contentBlockModel.DefinitionId,
                                                                     contentBlockModel.LayoutId);

            yield return blockModel;
        }
    }

    private IEnumerable<string> GetViewPaths(ContentBlocks contentBlocks) {
        foreach (var blockItem in contentBlocks.Blocks) {
            yield return $"Views/Blocks/{blockItem.Content.ContentType.Alias.Pascalize()}/Default.cshtml";
        }
    }
}
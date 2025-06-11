using Humanizer;
using Microsoft.AspNetCore.Http;
using Perplex.ContentBlocks.Rendering;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks.Perplex;

public class PerplexBlocksRenderer : BlocksRenderer<ContentBlocks> {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PerplexBlocksRenderer(IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override IEnumerable<IBlockViewModel> GetViewModels(ContentBlocks contentBlocks) {
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

    protected override IEnumerable<string> GetViewPaths(ContentBlocks contentBlocks) {
        foreach (var blockItem in contentBlocks.Blocks) {
            yield return $"Views/Blocks/{blockItem.Content.ContentType.Alias.Pascalize()}/Default.cshtml";
        }
    }
}
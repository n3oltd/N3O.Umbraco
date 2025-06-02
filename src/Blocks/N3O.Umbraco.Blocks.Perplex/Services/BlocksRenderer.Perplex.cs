using Microsoft.AspNetCore.Html;
using Perplex.ContentBlocks.Rendering;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks.Perplex;

public class PerplexBlocksRenderer : BlocksRenderer<ContentBlocks> {
    private readonly IContentBlockRenderer _contentBlockRenderer;

    public PerplexBlocksRenderer(IContentBlockRenderer contentBlockRenderer) {
        _contentBlockRenderer = contentBlockRenderer;
    }

    protected override IEnumerable<IBlockViewModel> GetViewModels(ContentBlocks contentBlocks) {
        var factory = new PerplexBlockViewModelFactory<IPublishedElement, EmailBuilderBlockViewModel>(_httpContextAccessor,
                                                                                                      (_, p) => new EmailBuilderBlockViewModel(p, composition, _assets, _emailLinkBuilder, _styleContext));
    }

    protected override IEnumerable<string> GetViewPaths(ContentBlocks contentBlocks) {
        throw new System.NotImplementedException();
    }
}
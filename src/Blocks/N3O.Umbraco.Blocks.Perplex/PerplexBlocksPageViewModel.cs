using N3O.Umbraco.Pages;
using Perplex.ContentBlocks.Rendering;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks.Perplex;

public interface IPerplexBlocksPageViewModel : IPageViewModel {
    IContentBlocks Blocks { get; }
}

public interface IPerplexBlocksPageViewModel<out TPage> : IPerplexBlocksPageViewModel, IPageViewModel<TPage>
    where TPage : IPublishedContent { }

public class PerplexBlocksPageViewModel<TPage> : PageViewModel<TPage>, IPerplexBlocksPageViewModel<TPage>
    where TPage : IPublishedContent {
    public PerplexBlocksPageViewModel(PageParameters<TPage> parameters) : base(parameters) { }

    public IContentBlocks Blocks => Content.GetProperty("blocks")?.GetValue() as IContentBlocks;
}

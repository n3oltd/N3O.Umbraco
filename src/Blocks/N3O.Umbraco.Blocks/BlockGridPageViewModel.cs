using N3O.Umbraco.Pages;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public interface IBlockGridPageViewModel : IPageViewModel {
    BlockGridModel Blocks { get; }
}

public interface IBlockGridPageViewModel<out TPage> : IBlockGridPageViewModel, IPageViewModel<TPage>
    where TPage : IPublishedContent { }

public class BlockGridPageViewModel<TPage> : PageViewModel<TPage>, IBlockGridPageViewModel<TPage>
    where TPage : IPublishedContent {
    public BlockGridPageViewModel(PageParameters<TPage> parameters) : base(parameters) { }

    public BlockGridModel Blocks => Content.GetProperty("blocks")?.GetValue() as BlockGridModel;
}

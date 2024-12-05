using N3O.Umbraco.Pages;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public interface IBlockListPageViewModel : IPageViewModel {
    IEnumerable<BlockListItem> Blocks { get; }
}

public interface IBlockListPageViewModel<out TPage> : IBlockListPageViewModel, IPageViewModel<TPage>
    where TPage : IPublishedContent { }

public class BlockListPageViewModel<TPage> : PageViewModel<TPage>, IBlockListPageViewModel<TPage>
    where TPage : IPublishedContent {
    public BlockListPageViewModel(PageParameters<TPage> parameters) : base(parameters) { }

    public IEnumerable<BlockListItem> Blocks => Content.GetProperty("blocks")?.GetValue() as IEnumerable<BlockListItem>;
}

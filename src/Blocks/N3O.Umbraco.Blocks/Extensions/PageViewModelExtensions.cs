using N3O.Umbraco.Pages;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.Blocks;

namespace N3O.Umbraco.Blocks.Extensions;

public static class PageViewModelExtensions {
    public static BlockGridModel BlockGrid(this IPageViewModel pageViewModel) {
        return pageViewModel.Content.GetProperty("blocks")?.GetValue() as BlockGridModel;
    }
    
    public static IEnumerable<BlockListItem> BlockListItems(this IPageViewModel pageViewModel) {
        return pageViewModel.Content.GetProperty("blocks")?.GetValue() as IEnumerable<BlockListItem>;
    }
}
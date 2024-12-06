using N3O.Umbraco.Pages;
using Perplex.ContentBlocks.Rendering;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.Blocks;

namespace N3O.Umbraco.Blocks.Perplex.Extensions;

public static class PageViewModelExtensions {
    public static IContentBlocks ContentBlocks(this IPageViewModel pageViewModel) {
        return pageViewModel.Content.GetProperty("blocks")?.GetValue() as IContentBlocks;
    }
}
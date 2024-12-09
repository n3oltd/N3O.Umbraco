using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.Blocks;

namespace N3O.Umbraco.Blocks.Extensions;

public static class BlockListModelExtensions {
    public static T As<T>(this BlockListModel blockListModel) {
        return (T) blockListModel.OrEmpty().SingleOrDefault()?.Content;
    }
    
    public static IReadOnlyList<T> AsCollectionOf<T>(this BlockListModel blockListModel) {
        return blockListModel.OrEmpty().Select(x => (T) x.Content).ToList();
    }
}
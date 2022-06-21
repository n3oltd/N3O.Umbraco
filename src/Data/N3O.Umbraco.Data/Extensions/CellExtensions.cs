using N3O.Umbraco.Data.Models;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Extensions;

public static class CellExtensions {
    public static T GetValue<T>(this Cell cell) {
        return (T) cell.Value;
    }

    public static void SetTitleMetadata(this Cell cell, string title) {
        cell.Metadata[DataConstants.MetadataKeys.Cells.Title] = title.Yield();
    }
}

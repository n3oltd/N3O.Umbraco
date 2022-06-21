using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Services;

public interface IColumnVisibility {
    bool IsVisible(Column column);
}

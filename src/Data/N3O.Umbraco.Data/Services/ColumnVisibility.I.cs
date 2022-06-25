using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data;

public interface IColumnVisibility {
    bool IsVisible(Column column);
}

using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Models;

public interface IColumnHeading {
    string GetText(IFormatter formatter, int? columnIndex, Cell cell);
}

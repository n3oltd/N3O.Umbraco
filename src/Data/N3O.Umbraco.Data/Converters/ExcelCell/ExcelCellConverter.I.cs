using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public interface IExcelCellConverter {
    ExcelCell GetExcelCell(Cell cell, IFormatter formatter, string comment = null);
}

public interface IExcelCellConverter<T> : IExcelCellConverter {
    ExcelCell<T> GetExcelCell(Cell<T> cell, IFormatter formatter, string comment = null);
}

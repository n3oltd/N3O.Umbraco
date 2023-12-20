using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data;

public interface IExcelCellFormatter {
    ExcelCell FormatCell(Cell cell, IFormatter formatter);
}

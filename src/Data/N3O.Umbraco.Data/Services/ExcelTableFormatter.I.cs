using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data;

public interface IExcelTableFormatter {
    ExcelCell FormatCell(Column column, Cell cell);
    ExcelColumn FormatColumn(Column column);
}

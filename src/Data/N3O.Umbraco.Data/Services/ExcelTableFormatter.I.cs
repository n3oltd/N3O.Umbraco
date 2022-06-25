using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data;

public interface IExcelTableFormatter {
    public ExcelCell FormatCell(Column column, Cell cell);
    public ExcelColumn FormatColumn(Column column);
}

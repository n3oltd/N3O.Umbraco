using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public interface IExcelTable : ITable {
    new IReadOnlyList<ExcelColumn> Columns { get; }
    new IReadOnlyDictionary<CellAddress, ExcelCell> Cells { get; }

    ExcelCell this[ExcelColumn column, int row] { get; }
    new ExcelCell this[int columnNumber, int row] { get; }
    new IEnumerable<ExcelRow> Rows { get; }

    bool HasFooters { get; }
}

using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models {
    public interface ITable {
        string Name { get; }
        int ColumnCount { get; }
        int RowCount { get; }
        IReadOnlyList<Column> Columns { get; }
        IReadOnlyDictionary<CellAddress, Cell> Cells { get; }
        IEnumerable<Row> Rows { get; }

        Cell this[Column column, int row] { get; }
        Cell this[int columnNumber, int row] { get; }
    }
}
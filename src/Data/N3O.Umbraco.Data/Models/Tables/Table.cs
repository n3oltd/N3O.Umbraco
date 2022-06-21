using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Models;

public class Table : ITable {
    public Table(string name,
                 int columnCount,
                 int rowCount,
                 IReadOnlyList<Column> columns,
                 IReadOnlyDictionary<CellAddress, Cell> cells) {
        Name = name;
        ColumnCount = columnCount;
        RowCount = rowCount;
        Columns = columns;
        Cells = cells;
    }

    public string Name { get; }
    public int ColumnCount { get; }
    public int RowCount { get; }
    public IReadOnlyList<Column> Columns { get; }
    public IReadOnlyDictionary<CellAddress, Cell> Cells { get; }

    public Cell this[Column column, int row] {
        get {
            var cellAddress = new CellAddress(column, row);

            return Cells[cellAddress];
        }
    }

    public Cell this[int columnNumber, int row] {
        get {
            var column = Columns[columnNumber];

            return this[column, row];
        }
    }

    public IEnumerable<Row> Rows {
        get {
            for (var rowNumber = 0; rowNumber < RowCount; rowNumber++) {
                var cells = Enumerable.Range(0, ColumnCount)
                                      .Select(columnNumber => this[columnNumber, rowNumber]);

                var row = new Row(cells);

                yield return row;
            }
        }
    }
}

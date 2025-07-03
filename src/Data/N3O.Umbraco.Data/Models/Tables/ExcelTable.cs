using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Models;

public class ExcelTable : IExcelTable {
    private readonly ITable _table;
    private readonly Dictionary<CellAddress, ExcelCell> _cells = new();
    private readonly List<ExcelColumn> _columns = [];

    public ExcelTable(ITable table, IExcelTableFormatter tableFormatter) {
        _table = table;

        PopulateCells(table, tableFormatter);
    }

    private void PopulateCells(ITable table, IExcelTableFormatter tableFormatter) {
        foreach (var column in table.Columns) {
            var excelColumn = tableFormatter.FormatColumn(column);
            _columns.Add(excelColumn);

            for (var row = 0; row < table.RowCount; row++) {
                var cell = table[column, row];
                var excelCell = tableFormatter.FormatCell(column, cell);
                var excelCellAddress = new CellAddress(excelColumn, row);

                _cells[excelCellAddress] = excelCell;
            }
        }
    }

    public string Name => _table.Name;
    public int ColumnCount => _table.ColumnCount;
    public int RowCount => _table.RowCount;

    IReadOnlyList<Column> ITable.Columns => _table.Columns;
    IReadOnlyDictionary<CellAddress, Cell> ITable.Cells => _table.Cells;
    Cell ITable.this[Column column, int row] => _table[column, row];
    Cell ITable.this[int columnNumber, int row] => _table[columnNumber, row];

    public IReadOnlyList<ExcelColumn> Columns => _columns;
    public IReadOnlyDictionary<CellAddress, ExcelCell> Cells => _cells;

    public ExcelCell this[ExcelColumn column, int row] {
        get {
            var cellAddress = new CellAddress(column, row);

            return _cells[cellAddress];
        }
    }

    public ExcelCell this[int columnNumber, int row] {
        get {
            var column = Columns[columnNumber];

            return this[column, row];
        }
    }

    public IEnumerable<ExcelRow> Rows {
        get {
            for (var rowNumber = 0; rowNumber < RowCount; rowNumber++) {
                var cells = Enumerable.Range(0, ColumnCount)
                                      .Select(columnNumber => this[columnNumber, rowNumber]);

                var row = new ExcelRow(cells);

                yield return row;
            }
        }
    }

    IEnumerable<Row> ITable.Rows => Rows;

    public bool HasFooters => _columns.Any(x => x.FooterFunction != null);
}

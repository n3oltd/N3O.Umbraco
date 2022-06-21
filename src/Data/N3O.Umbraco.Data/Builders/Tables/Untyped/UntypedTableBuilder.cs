using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Builders;

public class UntypedTableBuilder : IUntypedTableBuilder {
    private readonly string _name;
    private readonly HashSet<IColumnRange> _columnRanges = new();
    private readonly Dictionary<Column, IColumnRange> _columnToRangeMap = new();
    private int _rowNumber;

    public UntypedTableBuilder(string name) {
        _name = name;
    }

    public void AddCell(IColumnRange columnRange, Cell cell) {
        _columnRanges.AddIfNotExists(columnRange);

        columnRange.AddCells(_rowNumber, cell);
    }

    public void AddValue(IColumnRange columnRange, object value) {
        _columnRanges.AddIfNotExists(columnRange);

        columnRange.AddValues(_rowNumber, value);
    }

    public void AddCells(IColumnRange columnRange, IEnumerable<Cell> cells) {
        _columnRanges.AddIfNotExists(columnRange);

        columnRange.AddCells(_rowNumber, cells);
    }

    public void AddValues(IColumnRange columnRange, IEnumerable values) {
        _columnRanges.AddIfNotExists(columnRange);

        columnRange.AddValues(_rowNumber, values);
    }

    public void NextRow() {
        _rowNumber++;
    }

    public ITable Build() {
        var columns = GetColumns();
        var columnCount = columns.Count;
        var rowCount = _rowNumber;
        var cells = GetCells(columns, rowCount);

        var table = new Table(_name, columnCount, rowCount, columns, cells);

        return table;
    }

    private IReadOnlyList<Column> GetColumns() {
        var columns = new List<Column>();

        _columnToRangeMap.Clear();

        foreach (var columnRange in _columnRanges.OrderBy(x => x.Order).ThenBy(x => _columnRanges.IndexOf(x))) {
            var columnRangeColumns = columnRange.GetColumns().ToList();

            columns.AddRange(columnRangeColumns);

            columnRangeColumns.Do(x => _columnToRangeMap.Add(x, columnRange));
        }

        return columns;
    }

    private IReadOnlyDictionary<CellAddress, Cell> GetCells(IReadOnlyList<Column> columns, int rowCount) {
        var cells = new Dictionary<CellAddress, Cell>();

        for (var row = 0; row < rowCount; row++) {
            foreach (var column in columns) {
                var address = new CellAddress(column, row);
                var cell = GetCell(address);

                cells[address] = cell;
            }
        }

        return cells;
    }

    private Cell GetCell(CellAddress address) {
        var columnRange = _columnToRangeMap[address.Column];

        var cell = columnRange.GetCell(address);

        return cell;
    }
}

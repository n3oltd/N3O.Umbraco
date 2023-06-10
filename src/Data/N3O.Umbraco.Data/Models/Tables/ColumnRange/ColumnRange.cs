using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Models;

public class ColumnRange<TValue> : IColumnRange {
    private readonly IFormatter _formatter;
    private readonly ILocalClock _localClock;
    private readonly ICellConverter<TValue> _cellConverter;
    private readonly IColumnHeading _columnHeading;
    private readonly Func<IFormatter, string> _getComment;
    private readonly RangeColumnSort _rangeColumnSort;
    private readonly CollectionLayout _collectionLayout;
    private readonly DataType _dataType;
    private readonly Dictionary<string, IEnumerable<object>> _columnMetadata;
    private readonly bool _hidden;
    private readonly AccessControlList _accessControlList;
    private readonly Dictionary<string, Column> _columns = new();
    private readonly Dictionary<CellAddress, Cell> _cells = new();
    private readonly IEnumerable<Attribute> _attributes;

    public ColumnRange(IFormatter formatter,
                       ILocalClock localClock,
                       ICellConverter<TValue> cellConverter,
                       IColumnHeading columnHeading,
                       Func<IFormatter, string> getComment,
                       RangeColumnSort rangeColumnSort,
                       CollectionLayout collectionLayout,
                       DataType dataType,
                       Dictionary<string, IEnumerable<object>> columnMetadata,
                       bool hidden,
                       AccessControlList accessControlList,
                       IEnumerable<Attribute> attributes,
                       int order) {
        _formatter = formatter;
        _localClock = localClock;
        _rangeColumnSort = rangeColumnSort;
        _collectionLayout = collectionLayout;
        _dataType = dataType;
        _cellConverter = cellConverter;
        _columnHeading = columnHeading;
        _getComment = getComment;
        _columnMetadata = columnMetadata;
        _hidden = hidden;
        _accessControlList = accessControlList;
        _attributes = attributes;
        Order = order;
    }

    public void AddCells(int row, object cells) {
        if (cells is Cell singleCell) {
            cells = singleCell.Yield();
        }
        
        foreach (var (cell, columnIndex) in ((IEnumerable<Cell>) cells).SelectWithIndex()) {
            if (cell == null) {
                throw new ArgumentNullException(nameof(cell), $"Null cell passed to {nameof(AddCells)}");
            }
            
            var column = GetOrCreateColumn(columnIndex, cell);
            var cellAddress = new CellAddress(column, row);

            _cells[cellAddress] = cell;
        }
    }
    
    public void AddValues(int row, object value) {
        if (value is IEnumerable<TValue> enumerable) {
            AddValues(row, enumerable);
        } else {
            AddValue(row, (TValue) value);
        }
    }

    public Cell GetCell(CellAddress address) {
        var cell = _cells.GetValueOrDefault(address);

        return cell;
    }

    public IEnumerable<Column> GetColumns() {
        var columnKeys = _columns.Keys.ToList();

        _rangeColumnSort.Sort(columnKeys);

        var columns = columnKeys.Select(x => _columns[x]).ToList();

        return columns;
    }
    
    public int Order { get; }

    private void AddValue(int row, TValue value) {
        AddValue(row, null, value);
    }

    private void AddValues(int row, IEnumerable<TValue> values) {
        if (_collectionLayout.MultiColumn) {
            AddMultipleValues(row, values);
        } else {
            AddSingleValue(row, values, _collectionLayout.ValueSeparator);
        }
    }

    private void AddMultipleValues(int row, IEnumerable<TValue> values) {
        foreach (var (value, columnIndex) in values.SelectWithIndex()) {
            AddValue(row, columnIndex, value);
        }
    }

    private void AddSingleValue(int row, IEnumerable<TValue> values, string separator) {
        var textValues = new List<string>();

        foreach (var value in values) {
            var cell = _cellConverter.Convert(_formatter, _localClock, value, typeof(TValue));
            var text = cell.ToString(_formatter);

            textValues.Add(text);
        }

        var joinedText = string.Join(separator, textValues);
        var textCell = OurDataTypes.String.Cell(joinedText, null);
        var column = GetOrCreateColumn(null, textCell);
        var cellAddress = new CellAddress(column, row);

        _cells[cellAddress] = textCell;
    }

    private void AddValue(int row, int? columnIndex, TValue value) {
        var cell = _cellConverter.Convert(_formatter, _localClock, value, typeof(TValue));
        var column = GetOrCreateColumn(columnIndex, cell);
        var cellAddress = new CellAddress(column, row);

        _cells[cellAddress] = cell;
    }

    private Column GetOrCreateColumn(int? columnIndex, Cell cell) {
        var headingText = _columnHeading.GetText(_formatter, columnIndex, cell);

        var result = _columns.GetOrAdd(headingText, () => {
            var comment = _getComment?.Invoke(_formatter);
            
            var column = new Column(_dataType,
                                    headingText,
                                    comment,
                                    _formatter,
                                    _localClock,
                                    _hidden,
                                    _accessControlList,
                                    _attributes,
                                    _columnMetadata);

            return column;
        });

        return result;
    }
}

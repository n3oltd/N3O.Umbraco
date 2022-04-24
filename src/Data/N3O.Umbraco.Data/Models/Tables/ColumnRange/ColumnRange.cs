using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Models {
    public class ColumnRange<TValue> : IColumnRange {
        private readonly IFormatter _formatter;
        private readonly LocalizationSettings _localizationSettings;
        private readonly ILocalClock _localClock;
        private readonly ICellConverter<TValue> _cellConverter;
        private readonly IColumnHeading _columnHeading;
        private readonly Func<IFormatter, string> _getComment;
        private readonly RangeColumnSort _rangeColumnSort;
        private readonly CollectionLayout _collectionLayout;
        private readonly int _maxValues;
        private readonly DataType _dataType;
        private readonly IReadOnlyDictionary<string, IReadOnlyList<object>> _columnMetadata;
        private readonly bool _hidden;
        private readonly AccessControlList _accessControlList;
        private readonly Dictionary<string, Column> _columns = new();
        private readonly Dictionary<CellAddress, Cell> _cells = new();
        private readonly IEnumerable<Attribute> _attributes;

        public ColumnRange(IFormatter formatter,
                           LocalizationSettings localizationSettings,
                           ILocalClock localClock,
                           ICellConverter<TValue> cellConverter,
                           IColumnHeading columnHeading,
                           Func<IFormatter, string> getComment,
                           RangeColumnSort rangeColumnSort,
                           CollectionLayout collectionLayout,
                           int maxValues,
                           DataType dataType,
                           IReadOnlyDictionary<string, IReadOnlyList<object>> columnMetadata,
                           bool hidden,
                           AccessControlList accessControlList,
                           IEnumerable<Attribute> attributes) {
            _formatter = formatter;
            _localizationSettings = localizationSettings;
            _localClock = localClock;
            _rangeColumnSort = rangeColumnSort;
            _collectionLayout = collectionLayout;
            _maxValues = maxValues;
            _dataType = dataType;
            _cellConverter = cellConverter;
            _columnHeading = columnHeading;
            _getComment = getComment;
            _columnMetadata = columnMetadata;
            _hidden = hidden;
            _accessControlList = accessControlList;
            _attributes = attributes;
        }

        public void AddCells(int row, object value) {
            if (value is IEnumerable<TValue> enumerable) {
                AddCells(row, enumerable);
            } else {
                AddCell(row, (TValue)value);
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

        private void AddCell(int row, TValue value) {
            AddCell(row, null, value);
        }

        private void AddCellsAsync(int row, IEnumerable<TValue> values) {
            if (_collectionLayout.MultiColumn) {
                AddMultipleCells(row, values);
            } else {
                AddSingleCell(row, values, _collectionLayout.ValueSeparator);
            }
        }

        private void AddMultipleCells(int row, IEnumerable<TValue> values) {
            foreach (var (value, columnIndex) in values.Select((value, index) => (value, index))) {
                AddCell(row, columnIndex, value);
            }
        }

        private void AddSingleCell(int row, IEnumerable<TValue> values, string separator) {
            var textValues = new List<string>();

            foreach (var value in values) {
                var cell = _cellConverter.Convert(_formatter, _localClock, value, typeof(TValue));
                var text = cell.ToString(_formatter);

                textValues.Add(text);
            }

            var joinedText = string.Join(separator, textValues);
            var textCell = DataTypes.String.Cell(joinedText, null);
            var column = GetOrCreateColumn(null, textCell);
            var cellAddress = new CellAddress(column, row);

            _cells[cellAddress] = textCell;
        }

        private void AddCell(int row, int? columnIndex, TValue value) {
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
                                        _localizationSettings,
                                        _hidden,
                                        _accessControlList,
                                        _attributes,
                                        _columnMetadata);

                return column;
            });

            return result;
        }
    }
}
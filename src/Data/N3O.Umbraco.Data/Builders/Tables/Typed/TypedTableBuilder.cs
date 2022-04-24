using N3O.Umbraco.Data.Attributes;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace N3O.Umbraco.Data.Builders {
    public class TypedTableBuilder<TRow> : ITypedTableBuilder<TRow> {
        private readonly IColumnRangeBuilder _columnRangeBuilder;
        private readonly IUntypedTableBuilder _tableBuilder;
        private readonly List<IRowProperty<TRow>> _rowProperties = new();
        private bool _schemaCreated;
        private int _rowCount;

        public TypedTableBuilder(IColumnRangeBuilder columnRangeBuilder, string name) {
            _columnRangeBuilder = columnRangeBuilder;
            _tableBuilder = new UntypedTableBuilder(name);
        }

        public void AddRow(TRow row, Action<AddedRow<TRow>> rowAddedCallback = null) {
            if (!_schemaCreated) {
                CreateSchema();
            }

            foreach (var property in _rowProperties) {
                property.AddCells(row);
            }

            if (rowAddedCallback != null) {
                var args = new AddedRow<TRow>(_rowCount, row);

                rowAddedCallback(args);
            }

            _tableBuilder.NextRow();
            _rowCount++;
        }

        public void AddRows(IEnumerable<TRow> rows, Action<AddedRow<TRow>> rowAddedCallback = null) {
            foreach (var row in rows) {
                AddRow(row, rowAddedCallback);
            }
        }

        private void CreateSchema() {
            var properties = typeof(TRow).GetProperties()
                                            .Where(x => x.HasAttribute<ColumnRangeAttribute>())
                                            .OrderBy(x => x.GetCustomAttribute<ColumnRangeAttribute>().Order)
                                            .ThenBy(x => x.Name)
                                            .ToList();

            foreach (var property in properties) {
                var propertyType = property.PropertyType;
                var valueType = GetValueType(propertyType);

                var rowPropertyType = typeof(RowProperty<,,>).MakeGenericType(typeof(TRow), propertyType, valueType);
                var rowProperty = (IRowProperty<TRow>)Activator.CreateInstance(rowPropertyType,
                                                                               property,
                                                                               _columnRangeBuilder,
                                                                               _tableBuilder);

                rowProperty.CreateColumnRange();

                _rowProperties.Add(rowProperty);
            }

            _schemaCreated = true;
        }

        private Type GetValueType(Type propertyType) {
            var valueType = propertyType;

            if (propertyType.IsCollectionType()) {
                valueType = propertyType.GetCollectionType();
            }

            return valueType;
        }

        public ITable Build() {
            var table = _tableBuilder.Build();

            return table;
        }
    }
}
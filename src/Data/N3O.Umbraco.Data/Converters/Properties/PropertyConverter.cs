using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Converters {
    public abstract class PropertyConverter<T> : PropertyConverter<T, T> {
        protected PropertyConverter(IColumnRangeBuilder columnRangeBuilder) : base(columnRangeBuilder) { }
    }
    
    public abstract class PropertyConverter<TImport, TExport> : IPropertyConverter {
        private readonly IColumnRangeBuilder _columnRangeBuilder;
        private readonly Dictionary<string, IColumnRange> _columnRanges = new(StringComparer.InvariantCultureIgnoreCase);

        protected PropertyConverter(IColumnRangeBuilder columnRangeBuilder) {
            _columnRangeBuilder = columnRangeBuilder;
        }

        public void Export(IUntypedTableBuilder tableBuilder,
                           IEnumerable<IPropertyConverter> converters,
                           int columnOrder,
                           string columnTitlePrefix,
                           IContentProperty contentProperty,
                           UmbracoPropertyInfo propertyInfo) {
            var cells = GetCells(contentProperty, propertyInfo);

            foreach (var (cell, index) in cells.SelectWithIndex()) {
                var title = propertyInfo.GetColumnTitle(columnTitlePrefix);

                if (index != 0) {
                    title += $" {index + 1}";
                }
                
                var columnRange = GetOrAddColumnRange(cell.Type, columnOrder + index, title);

                tableBuilder.AddCell(columnRange, cell);
            }
        }

        public virtual IReadOnlyList<Column> GetColumns(IEnumerable<IPropertyConverter> converters,
                                                        UmbracoPropertyInfo propertyInfo,
                                                        string columnTitlePrefix) {
            var columnTemplate = new ColumnTemplate(propertyInfo.GetColumnTitle(columnTitlePrefix),
                                                    Math.Min(GetMaxValues(propertyInfo),
                                                             DataConstants.Limits.Columns.MaxValues),
                                                    propertyInfo);

            return _columnRangeBuilder.GetColumns(columnTemplate);
        }

        public abstract void Import(IContentBuilder contentBuilder,
                                    IEnumerable<IPropertyConverter> converters,
                                    IParser parser,
                                    ErrorLog errorLog,
                                    string columnTitlePrefix,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<ImportField> fields);

        public abstract bool IsConverter(UmbracoPropertyInfo propertyInfo);

        protected abstract IEnumerable<Cell<TExport>> GetCells(IContentProperty contentProperty,
                                                               UmbracoPropertyInfo propertyInfo);
        
        protected virtual int GetMaxValues(UmbracoPropertyInfo propertyInfo) => 1;
        
        protected IEnumerable<Cell<TExport>> ExportValue<TProperty>(IContentProperty contentProperty,
                                                                    Func<TProperty, Cell<TExport>> toCell) {
            return ExportValue(contentProperty, x => (TProperty) x, toCell);
        }

        protected IEnumerable<Cell<TExport>> ExportValue<TProperty>(IContentProperty contentProperty,
                                                                    Func<object, TProperty> convertValue,
                                                                    Func<TProperty, Cell<TExport>> toCell) {
            var value = convertValue(contentProperty.Value);

            return toCell(value).Yield();
        }
        
        protected IEnumerable<Cell<TExport>> ExportValues<TProperty>(IContentProperty contentProperty,
                                                                     Func<TProperty, Cell<TExport>> toCell) {
            var values = (IEnumerable<TProperty>) contentProperty.Value;

            return values.OrEmpty().Select(toCell);
        }
        
        protected void Import(ErrorLog errorLog,
                              UmbracoPropertyInfo propertyInfo,
                              IEnumerable<ImportField> fields,
                              Func<string, ParseResult<TImport>> parse,
                              Action<string, TImport> setContent) {
            if (fields.OrEmpty().Count() > 1) {
                throw new Exception($"Multiple values passed to {nameof(Import)}");
            }

            ImportAll(errorLog,
                      propertyInfo,
                      fields,
                      parse,
                      (alias, values) => setContent(alias, values.Single()));
        }
        
        protected void ImportAll(ErrorLog errorLog,
                                 UmbracoPropertyInfo propertyInfo,
                                 IEnumerable<ImportField> fields,
                                 Func<string, ParseResult<TImport>> parse,
                                 Action<string, IEnumerable<TImport>> setContent) {
            var values = new List<TImport>();

            foreach (var field in fields) {
                var parseResult = parse(field.Value);

                if (parseResult.Success) {
                    values.Add(parseResult.Value);
                } else {
                    errorLog.AddError<Strings>(s => s.ParsingFailed_2, field.Value, field.Name);
                }
            }

            if (values.HasAny()) {
                setContent(propertyInfo.Type.Alias, values);
            }
        }
        
        private IColumnRange GetOrAddColumnRange(DataType dataType, int columnOrder, string title) {
            return _columnRanges.GetOrAdd(title,
                                          () => _columnRangeBuilder.OfType<TExport>(dataType)
                                                                   .Title(title)
                                                                   .SetOrder(columnOrder)
                                                                   .PreserveColumnOrder()
                                                                   .Build());
        }

        public class Strings : CodeStrings {
            public string ParsingFailed_2 => $"The value {"{0}".Quote()} is invalid for {"{1}".Quote()}";
        }
    }
}
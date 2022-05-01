using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters {
    public class NestedContentPropertyConverter : IPropertyConverter {
        private readonly IColumnRangeBuilder _columnRangeBuilder;
        private readonly Dictionary<string, IColumnRange> _columnRanges = new(StringComparer.InvariantCultureIgnoreCase);
        private readonly string _orderColumnTitle;

        public NestedContentPropertyConverter(IColumnRangeBuilder columnRangeBuilder, IFormatter formatter) {
            _columnRangeBuilder = columnRangeBuilder;
            
            _orderColumnTitle = formatter.Text.Format<DataStrings>(s => s.OrderColumnTitle);
        }
        
        public bool IsConverter(UmbracoPropertyInfo propertyInfo) {
            return propertyInfo.Type
                               .PropertyEditorAlias
                               .EqualsInvariant(UmbracoPropertyEditors.Aliases.NestedContent);
        }

        public void Export(IUntypedTableBuilder tableBuilder,
                           IEnumerable<IPropertyConverter> converters,
                           string columnTitlePrefix,
                           IContentProperty contentProperty,
                           UmbracoPropertyInfo propertyInfo) {
            var nestedContentConfiguration = propertyInfo.DataType.ConfigurationAs<NestedContentConfiguration>();

            foreach (var (nestedContentProperties, index) in ((NestedContentProperty) contentProperty).OrEmpty(x => x.Value)
                                                                                                      .SelectWithIndex()) {
                var nestedContentInfo = propertyInfo.NestedContent
                                                    .Single(x => x.ContentType
                                                                  .Alias
                                                                  .EqualsInvariant(nestedContentProperties.ContentTypeAlias));

                var nestedColumnTitlePrefix = GetColumnTitlePrefix(propertyInfo,
                                                                   nestedContentInfo,
                                                                   index + 1,
                                                                   columnTitlePrefix);
                
                foreach (var nestedPropertyInfo in nestedContentInfo.Properties) {
                    var converter = nestedPropertyInfo.GetPropertyConverter(converters);
                    var nestedContentProperty = nestedContentProperties.GetPropertyByAlias(nestedPropertyInfo.Type.Alias);

                    converter.Export(tableBuilder,
                                     converters,
                                     nestedColumnTitlePrefix,
                                     nestedContentProperty,
                                     nestedPropertyInfo);
                }

                if (!nestedContentConfiguration.ContentTypes.IsSingle()) {
                    var orderColumnRange = GetOrAddColumnRange<int?>(OurDataTypes.Integer,
                                                                     GetOrderColumnTitle(nestedColumnTitlePrefix));
                    
                    tableBuilder.AddValue(orderColumnRange, index + 1);
                }
            }
        }

        public IReadOnlyList<Column> GetColumns(IEnumerable<IPropertyConverter> converters,
                                                UmbracoPropertyInfo propertyInfo,
                                                string columnTitlePrefix) {
            var columns = new List<Column>();
            var maxValues = GetMaxValues(propertyInfo);
            var nestedContentConfiguration = propertyInfo.DataType.ConfigurationAs<NestedContentConfiguration>();

            foreach (var nestedContent in propertyInfo.NestedContent) {
                for (var i = 1; i <= maxValues; i++) {
                    var nestedColumnTitlePrefix = GetColumnTitlePrefix(propertyInfo,
                                                                       nestedContent,
                                                                       i,
                                                                       columnTitlePrefix);

                    foreach (var nestedPropertyInfo in nestedContent.Properties) {
                        columns.AddRange(nestedPropertyInfo.GetColumns(converters, nestedColumnTitlePrefix));
                    }
                    
                    if (!nestedContentConfiguration.ContentTypes.IsSingle()) {
                        var orderColumnRange = GetOrAddColumnRange<int?>(OurDataTypes.Integer,
                                                                         GetOrderColumnTitle(nestedColumnTitlePrefix));

                        orderColumnRange.AddValues(0, null);
                        
                        columns.AddRange(orderColumnRange.GetColumns());
                    }
                }
            }

            return columns;
        }

        public void Import(IContentBuilder contentBuilder,
                           IEnumerable<IPropertyConverter> converters,
                           IParser parser,
                           ErrorLog errorLog,
                           string columnTitlePrefix,
                           UmbracoPropertyInfo propertyInfo,
                           IEnumerable<ImportField> fields) {
            var maxValues = GetMaxValues(propertyInfo);
            var nestedPropertyBuilder = contentBuilder.Nested(propertyInfo.Type.Alias);
            var nestedContentConfiguration = propertyInfo.DataType.ConfigurationAs<NestedContentConfiguration>();

            foreach (var nestedContent in propertyInfo.NestedContent) {
                for (var i = 1; i <= maxValues; i++) {
                    var nestedColumnTitlePrefix = GetColumnTitlePrefix(propertyInfo,
                                                                       nestedContent,
                                                                       i,
                                                                       columnTitlePrefix);

                    int? order = null;
                    
                    if (!nestedContentConfiguration.ContentTypes.IsSingle()) {
                        var orderColumnTitle = GetOrderColumnTitle(nestedColumnTitlePrefix);
                        
                        var orderField = fields.Single(x => x.Name.EqualsInvariant(orderColumnTitle));
                        order = orderField.Value.TryParseAs<int>();
                    }

                    IContentBuilder nestedContentBuilder = null;
                    
                    foreach (var nestedPropertyInfo in nestedContent.Properties) {
                        var nestedColumnTitle = nestedPropertyInfo.GetColumnTitle(nestedColumnTitlePrefix);

                        var nestedFields = fields.Where(f => f.Name.InvariantStartsWith(nestedColumnTitle)).ToList();

                        var converter = nestedPropertyInfo.GetPropertyConverter(converters);

                        nestedContentBuilder ??= nestedPropertyBuilder.Add(nestedContent.ContentType.Alias, order);

                        converter.Import(nestedContentBuilder,
                                         converters,
                                         parser,
                                         errorLog,
                                         nestedColumnTitle,
                                         nestedPropertyInfo,
                                         nestedFields);
                    }
                }
            }
        }
        
        private int GetMaxValues(UmbracoPropertyInfo propertyInfo) {
            var configuration = propertyInfo.DataType.ConfigurationAs<NestedContentConfiguration>();

            if (configuration.MaxItems == null || configuration.MaxItems == 0) {
                return DataConstants.Limits.Columns.MaxValues;
            } else {
                return configuration.MaxItems.GetValueOrThrow();
            }
        }
        
        private IColumnRange GetOrAddColumnRange<T>(DataType dataType, string title) {
            return _columnRanges.GetOrAdd(title,
                                          () => _columnRangeBuilder.OfType<T>(dataType)
                                                                   .Title(title)
                                                                   .PreserveColumnOrder()
                                                                   .Build());
        }
        
        private string GetColumnTitlePrefix(UmbracoPropertyInfo propertyInfo,
                                            NestedContentInfo nestedContentInfo,
                                            int i,
                                            string columnTitlePrefix) {
            var titlePrefix = propertyInfo.GetColumnTitle(columnTitlePrefix);

            if (i == 1) {
                titlePrefix += " // ";
            } else {
                titlePrefix += $" {i} // ";
            }
            
            if (!propertyInfo.NestedContent.IsSingle()) {
                titlePrefix += $"  {nestedContentInfo.ContentType.Name}: ";
            }

            return titlePrefix;
        }

        private string GetOrderColumnTitle(string columnTitlePrefix) {
            return $"{columnTitlePrefix} {_orderColumnTitle}";
        }
    }
}
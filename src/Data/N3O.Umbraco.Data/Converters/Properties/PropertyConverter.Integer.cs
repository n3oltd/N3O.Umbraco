using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters {
    public class IntegerPropertyConverter : PropertyConverter<long?> {
        public IntegerPropertyConverter(IColumnRangeBuilder columnRangeBuilder) : base(columnRangeBuilder) { }
        
        public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
            return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(UmbracoPropertyEditors.Aliases.Integer);
        }

        protected override IEnumerable<Cell<long?>> GetCells(IContentProperty contentProperty,
                                                             UmbracoPropertyInfo propertyInfo) {
            return ExportValue(contentProperty, ConvertToNullableLong, x => OurDataTypes.Integer.Cell(x));
        }

        public override void Import(IContentBuilder contentBuilder,
                                    IEnumerable<IPropertyConverter> converters,
                                    IParser parser,
                                    ErrorLog errorLog,
                                    string columnTitlePrefix,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<ImportField> fields) {
            Import(errorLog,
                   propertyInfo,
                   fields,
                   s => parser.Integer.Parse(s, OurDataTypes.Integer.GetClrType()),
                   (alias, value) => contentBuilder.Numeric(alias).SetInteger(value));
        }

        private long? ConvertToNullableLong(object value) {
            if (value == null) {
                return null;
            } else if (value is int intValue) {
                return intValue;
            } else if (value is long longValue) {
                return longValue;
            } else {
                throw UnrecognisedValueException.For(value);
            }
        }
    }
}
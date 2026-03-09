using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters;

public class BooleanPropertyConverter : PropertyConverter<bool?> {
    public BooleanPropertyConverter(IColumnRangeBuilder columnRangeBuilder) : base(columnRangeBuilder) { }
    
    public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(UmbracoPropertyEditors.Aliases.Boolean);
    }

    protected override IEnumerable<Cell<bool?>> GetCells(IContentProperty contentProperty,
                                                         UmbracoPropertyInfo propertyInfo) {
        return ExportValue(contentProperty, ConvertToNullableBool, x => OurDataTypes.Bool.Cell(x));
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
               s => parser.Bool.Parse(s, OurDataTypes.Bool.GetClrType()),
               (alias, value) => contentBuilder.Boolean(alias).Set(value));
    }
    
    private bool? ConvertToNullableBool(object value) {
        if (value == null) {
            return null;
        } else if (value is int intValue) {
            return intValue != 0;
        } else if (value is long longValue) {
            return longValue != 0;
        } else if (value is bool boolValue) {
            return boolValue;
        } else {
            throw UnrecognisedValueException.For(value);
        }
    }
}

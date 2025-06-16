using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters;

public class LabelPropertyConverter : PropertyConverter<string> {
    public LabelPropertyConverter(IColumnRangeBuilder columnRangeBuilder) : base(columnRangeBuilder) { }
    
    public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(UmbracoPropertyEditors.Aliases.Label);
    }

    protected override IEnumerable<Cell<string>> GetCells(IContentProperty contentProperty,
                                                          UmbracoPropertyInfo propertyInfo) {
        var labelConfiguration = propertyInfo.DataType.ConfigurationAs<LabelConfiguration>();

        switch (labelConfiguration.ValueType) {
            case ValueTypes.String:
            case ValueTypes.Text:
                return ExportValue<string>(contentProperty, x => OurDataTypes.String.Cell(x));
            
            case ValueTypes.Decimal:
                if (contentProperty.Value is decimal || contentProperty.Value == null) {
                    return ExportValue<decimal?>(contentProperty, x => OurDataTypes.String.Cell(x?.ToString()));
                } else if (contentProperty.Value is double) {
                    return ExportValue<double?>(contentProperty, x => OurDataTypes.String.Cell(x?.ToString()));
                } else {
                    throw UnrecognisedValueException.For(contentProperty.Value);
                }
            
            case ValueTypes.Date:
                return ExportValue<DateOnly?>(contentProperty, x => OurDataTypes.String.Cell(x?.ToString()));
                
            case ValueTypes.DateTime:
                return ExportValue<DateTime?>(contentProperty, x => OurDataTypes.String.Cell(x?.ToString()));
            
            case ValueTypes.Time:
                return ExportValue<TimeOnly?>(contentProperty, x => OurDataTypes.String.Cell(x?.ToString()));
            
            case ValueTypes.Integer:
                return ExportValue<int?>(contentProperty, x => OurDataTypes.String.Cell(x?.ToString()));

            case ValueTypes.Bigint:
                return ExportValue<long?>(contentProperty, x => OurDataTypes.String.Cell(x?.ToString()));
            
            default:
                throw UnrecognisedValueException.For(labelConfiguration.ValueType);
        }
    }

    public override void Import(IContentBuilder contentBuilder,
                                IEnumerable<IPropertyConverter> converters,
                                IParser parser,
                                ErrorLog errorLog,
                                string columnTitlePrefix,
                                UmbracoPropertyInfo propertyInfo,
                                IEnumerable<ImportField> fields) {
        var labelConfiguration = propertyInfo.DataType.ConfigurationAs<LabelConfiguration>();
        
        switch (labelConfiguration.ValueType) {
            case ValueTypes.String:
            case ValueTypes.Text:
                Import(errorLog,
                       propertyInfo,
                       fields,
                       s => parser.String.Parse(s, OurDataTypes.String.GetClrType()),
                       (alias, value) => contentBuilder.Label(alias).Set(value));
                break;
            
            case ValueTypes.Decimal:
                Import(errorLog,
                       propertyInfo,
                       fields,
                       s => parser.String.Parse(s, OurDataTypes.Decimal.GetClrType()),
                       (alias, value) => contentBuilder.Label(alias).Set(value));
                break;
            
            case ValueTypes.Date:
                Import(errorLog,
                       propertyInfo,
                       fields,
                       s => parser.String.Parse(s, OurDataTypes.Date.GetClrType()),
                       (alias, value) => contentBuilder.Label(alias).Set(value));
                break;
            
            case ValueTypes.DateTime:
                Import(errorLog,
                       propertyInfo,
                       fields,
                       s => parser.String.Parse(s, OurDataTypes.DateTime.GetClrType()),
                       (alias, value) => contentBuilder.Label(alias).Set(value));
                break;
            
            case ValueTypes.Time:
                Import(errorLog,
                       propertyInfo,
                       fields,
                       s => parser.String.Parse(s, OurDataTypes.Time.GetClrType()),
                       (alias, value) => contentBuilder.Label(alias).Set(value));
                break;
            
            case ValueTypes.Integer:
            case ValueTypes.Bigint:
                Import(errorLog,
                       propertyInfo,
                       fields,
                       s => parser.String.Parse(s, OurDataTypes.Integer.GetClrType()),
                       (alias, value) => contentBuilder.Label(alias).Set(value));
                break;
            
            default:
                throw UnrecognisedValueException.For(labelConfiguration.ValueType);
        }
    }
}

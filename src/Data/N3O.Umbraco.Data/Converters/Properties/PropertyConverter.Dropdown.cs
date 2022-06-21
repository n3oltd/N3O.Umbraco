using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters;

public class DropdownPropertyConverter : PropertyConverter<string> {
    public DropdownPropertyConverter(IColumnRangeBuilder columnRangeBuilder) : base(columnRangeBuilder) { }
    
    public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        return propertyInfo.Type
                           .PropertyEditorAlias
                           .EqualsInvariant(UmbracoPropertyEditors.Aliases.DropDownListFlexible);
    }

    protected override IEnumerable<Cell<string>> GetCells(IContentProperty contentProperty,
                                                          UmbracoPropertyInfo propertyInfo) {
        return ExportValue<string>(contentProperty, ConvertToString, x => OurDataTypes.String.Cell(x));
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
               s => parser.String.Parse(s, OurDataTypes.String.GetClrType()),
               (alias, value) => contentBuilder.Dropdown(alias).Set(value));
    }

    private string ConvertToString(object value) {
        if (value == null) {
            return null;
        } else if (value is string s) {
            return s;
        } else if (value is IEnumerable<string> strings) {
            return string.Join(" // ", strings);
        } else {
            throw UnrecognisedValueException.For(value);
        }
    }
}

using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters;

public class EmailPropertyConverter : PropertyConverter<string> {
    public EmailPropertyConverter(IColumnRangeBuilder columnRangeBuilder) : base(columnRangeBuilder) { }
    
    public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(UmbracoPropertyEditors.Aliases.EmailAddress);
    }

    protected override IEnumerable<Cell<string>> GetCells(IContentProperty contentProperty,
                                                          UmbracoPropertyInfo propertyInfo) {
        return ExportValue<string>(contentProperty, x => OurDataTypes.String.Cell(x));
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
               (alias, value) => contentBuilder.Email(alias).Set(value));
    }
}

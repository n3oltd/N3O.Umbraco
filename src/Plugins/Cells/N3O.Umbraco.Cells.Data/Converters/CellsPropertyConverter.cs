using N3O.Umbraco.Cells.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Cells.Data.Converters;

public class CellsPropertyConverter : PropertyConverter<string> {
    public CellsPropertyConverter(IColumnRangeBuilder columnRangeBuilder) : base(columnRangeBuilder) { }
    
    public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(CellsConstants.PropertyEditorAlias);
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
        var parseResult = parser.String.Parse(fields.Single().Value, OurDataTypes.String.GetClrType());

        if (parseResult.Value.HasValue()) {
            var modelValue = JsonConvert.DeserializeObject<object[][]>(parseResult.Value);

            contentBuilder.Cells(propertyInfo.Type.Alias).Set(modelValue);
        }
    }
}

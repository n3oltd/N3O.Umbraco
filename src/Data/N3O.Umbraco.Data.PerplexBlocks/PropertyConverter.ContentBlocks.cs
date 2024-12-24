using N3O.Umbraco.Blocks.Perplex.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Perplex.ContentBlocks.PropertyEditor.ModelValue;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;
using ContentBlocksConstants = Perplex.ContentBlocks.Constants;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters;

public class ContentBlocksPropertyConverter : PropertyConverter<string> {
    public ContentBlocksPropertyConverter(IColumnRangeBuilder columnRangeBuilder) : base(columnRangeBuilder) { }

    public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(ContentBlocksConstants.PropertyEditor.Alias);
    }

    protected override IEnumerable<Cell<string>> GetCells(IContentProperty contentProperty,
                                                          UmbracoPropertyInfo propertyInfo) {
        return OurDataTypes.String.Cell((contentProperty as ElementsProperty)?.Json).Yield();
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
            var modelValue = JsonConvert.DeserializeObject<ContentBlocksModelValue>(parseResult.Value);
            
            contentBuilder.PerplexBlocks(propertyInfo.Type.Alias).Set(modelValue);
        }
    }
}

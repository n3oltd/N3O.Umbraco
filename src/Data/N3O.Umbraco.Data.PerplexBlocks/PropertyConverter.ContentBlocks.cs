using N3O.Umbraco.Blocks.Perplex.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
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
        var field = fields.Single();
        var parseResult = parser.String.Parse(field.Value, OurDataTypes.String.GetClrType());

        if (!parseResult.Value.HasValue()) {
            return;
        } else if (IsContentBlocksJson(parseResult.Value)) {
            contentBuilder.PerplexBlocks(propertyInfo.Type.Alias).SetJson(parseResult.Value);
        } else {
            errorLog.AddError<PropertyConverterStrings>(s => s.ParsingFailed_2, field.Value, field.Name);
        }
    }

    // The cell is stored verbatim, so it is checked only for well-formedness and the member every content
    // blocks value carries. More than one layout is valid, and Perplex owns which of them it reads.
    private static bool IsContentBlocksJson(string json) {
        try {
            return JsonNode.Parse(json) is JsonObject jsonObject && jsonObject.ContainsKey("blocks");
        } catch (JsonException) {
            return false;
        }
    }
}

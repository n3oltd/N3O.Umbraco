using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters;

public class MultiNodeTreePickerPropertyConverter : PropertyConverter<IPublishedContent> {
    private static readonly string EditorAlias = UmbracoPropertyEditors.Aliases.MultiNodeTreePicker; 
    private readonly IContentHelper _contentHelper;

    public MultiNodeTreePickerPropertyConverter(IColumnRangeBuilder columnRangeBuilder,
                                                IContentHelper contentHelper)
        : base(columnRangeBuilder) {
        _contentHelper = contentHelper;
    }
    
    public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        if (!propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(EditorAlias)) {
            return false;
        }
        
        var configuration = propertyInfo.DataType.ConfigurationAs<MultiNodePickerConfiguration>();
        var parentId = configuration.TreeSource?.StartNodeId?.ToId();

        if (parentId == null && configuration.TreeSource.HasValue(x => x.StartNodeQuery)) {
            // As IPublishedQuery.ContentAtXPath() does not work without variables such as $root, $site etc.
            return false;
        }

        return true;
    }

    protected override IEnumerable<Cell<IPublishedContent>> GetCells(IContentProperty contentProperty,
                                                                     UmbracoPropertyInfo propertyInfo) {
        IEnumerable<IPublishedContent> values;

        if (GetMaxValues(propertyInfo) == 1) {
            values = _contentHelper.GetMultiNodeTreePickerValue<IPublishedContent>(contentProperty).Yield();
        } else {
            values = _contentHelper.GetMultiNodeTreePickerValues<IPublishedContent>(contentProperty);
        }

        return values.ExceptNull().Select(x => OurDataTypes.PublishedContent.Cell(x, x.GetType()));
    }

    public override void Import(IContentBuilder contentBuilder,
                                IEnumerable<IPropertyConverter> converters,
                                IParser parser,
                                ErrorLog errorLog,
                                string columnTitlePrefix,
                                UmbracoPropertyInfo propertyInfo,
                                IEnumerable<ImportField> fields) {
        ImportAll(errorLog,
                  propertyInfo,
                  fields,
                  s => Parse(parser, propertyInfo, s),
                  (alias, values) => contentBuilder.ContentPicker(alias).SetContent(values));
    }

    protected override int GetMaxValues(UmbracoPropertyInfo propertyInfo) {
        var configuration = propertyInfo.DataType.ConfigurationAs<MultiNodePickerConfiguration>();

        if (configuration.MaxNumber == 0) {
            return DataConstants.Limits.Columns.MaxValues;
        } else {
            return configuration.MaxNumber;
        }
    }
    
    private ParseResult<IPublishedContent> Parse(IParser parser, UmbracoPropertyInfo propertyInfo, string source) {
        var configuration = propertyInfo.DataType.ConfigurationAs<MultiNodePickerConfiguration>();
        var parentId = configuration.TreeSource?.StartNodeId?.ToId();

        return parser.PublishedContent.Parse(source, OurDataTypes.PublishedContent.GetClrType(), parentId);
    }
}

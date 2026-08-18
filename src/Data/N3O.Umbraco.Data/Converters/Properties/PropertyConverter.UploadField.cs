using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Utilities;
using System.Collections.Generic;
using Umbraco.Cms.Core.IO;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters;

// Native Umbraco.UploadField (inline single-file upload). The stored value is the file path string.
public class UploadFieldPropertyConverter : InlineMediaPropertyConverter {
    private static readonly string EditorAlias = UmbracoPropertyEditors.Aliases.UploadField;

    public UploadFieldPropertyConverter(IColumnRangeBuilder columnRangeBuilder,
                                        IUrlBuilder urlBuilder,
                                        MediaFileManager mediaFileManager,
                                        ILocalClock clock)
        : base(columnRangeBuilder, urlBuilder, mediaFileManager, clock) { }

    public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(EditorAlias);
    }

    protected override IEnumerable<Cell<string>> GetCells(IContentProperty contentProperty,
                                                          UmbracoPropertyInfo propertyInfo) {
        return ExportValue<string>(contentProperty, path => OurDataTypes.String.Cell(AbsoluteUrl(path)));
    }

    protected override string BuildValue(string urlPath) {
        return urlPath;
    }
}

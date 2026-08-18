using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Media;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters;

// Replaces the removed CropperPropertyConverter / UploaderPropertyConverter (both editors are retired in
// favour of native Umbraco.MediaPicker3). Export writes each picked item's media URL. Import is not
// supported: native MediaPicker3 references media-library nodes by key, so importing would have to create
// a media node per row and the target folder + de-duplication are per-site decisions (the offline
// media-migrate CLI takes them as flags). Existing content is migrated by that CLI; a populated media
// column on import is surfaced as an error rather than silently dropped.
public class MediaPickerPropertyConverter : PropertyConverter<string> {
    private static readonly string EditorAlias = UmbracoPropertyEditors.Aliases.MediaPicker3;
    private readonly IContentHelper _contentHelper;
    private readonly IMediaUrl _mediaUrl;

    public MediaPickerPropertyConverter(IColumnRangeBuilder columnRangeBuilder,
                                        IContentHelper contentHelper,
                                        IMediaUrl mediaUrl)
        : base(columnRangeBuilder) {
        _contentHelper = contentHelper;
        _mediaUrl = mediaUrl;
    }

    public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(EditorAlias);
    }

    protected override IEnumerable<Cell<string>> GetCells(IContentProperty contentProperty,
                                                          UmbracoPropertyInfo propertyInfo) {
        if (GetMaxValues(propertyInfo) == 1) {
            var media = _contentHelper.GetMediaPickerValue(contentProperty);

            return OurDataTypes.String.Cell(GetUrl(media)).Yield();
        } else {
            var media = _contentHelper.GetMediaPickerValues(contentProperty);

            return media.ExceptNull().Select(x => OurDataTypes.String.Cell(GetUrl(x)));
        }
    }

    public override void Import(IContentBuilder contentBuilder,
                                IEnumerable<IPropertyConverter> converters,
                                IParser parser,
                                ErrorLog errorLog,
                                string columnTitlePrefix,
                                UmbracoPropertyInfo propertyInfo,
                                IEnumerable<ImportField> fields) {
        foreach (var field in fields.OrEmpty().Where(x => x.Value.HasValue())) {
            errorLog.AddError<MediaImportStrings>(s => s.NotSupported_1, propertyInfo.GetColumnTitle(columnTitlePrefix));
        }
    }

    protected override int GetMaxValues(UmbracoPropertyInfo propertyInfo) {
        var configuration = propertyInfo.DataType.ConfigurationAs<MediaPicker3Configuration>();

        return configuration.Multiple ? DataConstants.Limits.Columns.MaxValues : 1;
    }

    private string GetUrl(MediaWithCrops media) {
        return media == null ? null : _mediaUrl.GetMediaUrl(media);
    }

    public class MediaImportStrings : CodeStrings {
        public string NotSupported_1 =>
            $"Importing media into {"{0}".Quote()} is not supported; add the media in the library and reference it";
    }
}

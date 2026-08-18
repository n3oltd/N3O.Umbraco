using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using UmbracoConstants = Umbraco.Cms.Core.Constants;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters;

// Replaces the removed CropperPropertyConverter / UploaderPropertyConverter (both editors are retired in
// favour of native Umbraco.MediaPicker3).
// Export: writes each picked item's media URL.
// Import: creates a normal media-library node from the uploaded file (IMediaService, so the configured
// storage provider — e.g. Azure Blob — stores it the same way as any other media) and references it. There
// is no distinction between imported media and media added through the backoffice.
public class MediaPickerPropertyConverter : PropertyConverter<Blob, string> {
    private static readonly string EditorAlias = UmbracoPropertyEditors.Aliases.MediaPicker3;

    private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase) {
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tiff", ".tif", ".svg"
    };

    private readonly IContentHelper _contentHelper;
    private readonly IMediaUrl _mediaUrl;
    private readonly IMediaService _mediaService;
    private readonly MediaFileManager _mediaFileManager;
    private readonly MediaUrlGeneratorCollection _mediaUrlGenerators;
    private readonly IShortStringHelper _shortStringHelper;
    private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;

    public MediaPickerPropertyConverter(IColumnRangeBuilder columnRangeBuilder,
                                        IContentHelper contentHelper,
                                        IMediaUrl mediaUrl,
                                        IMediaService mediaService,
                                        MediaFileManager mediaFileManager,
                                        MediaUrlGeneratorCollection mediaUrlGenerators,
                                        IShortStringHelper shortStringHelper,
                                        IContentTypeBaseServiceProvider contentTypeBaseServiceProvider)
        : base(columnRangeBuilder) {
        _contentHelper = contentHelper;
        _mediaUrl = mediaUrl;
        _mediaService = mediaService;
        _mediaFileManager = mediaFileManager;
        _mediaUrlGenerators = mediaUrlGenerators;
        _shortStringHelper = shortStringHelper;
        _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;
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
        Import(errorLog,
               propertyInfo,
               fields,
               s => parser.Blob.Parse(s, OurDataTypes.Blob.GetClrType()),
               (alias, blob) => contentBuilder.Raw(alias).Set(BuildValue(CreateMedia(blob))));
    }

    protected override int GetMaxValues(UmbracoPropertyInfo propertyInfo) {
        var configuration = propertyInfo.DataType.ConfigurationAs<MediaPicker3Configuration>();

        return configuration.Multiple ? DataConstants.Limits.Columns.MaxValues : 1;
    }

    private string GetUrl(MediaWithCrops media) {
        return media == null ? null : _mediaUrl.GetMediaUrl(media);
    }

    private Guid CreateMedia(Blob blob) {
        var mediaTypeAlias = IsImage(blob.Filename)
                                 ? UmbracoConstants.Conventions.MediaTypes.Image
                                 : UmbracoConstants.Conventions.MediaTypes.File;

        var media = _mediaService.CreateMedia(blob.Filename, UmbracoConstants.System.Root, mediaTypeAlias);

        blob.Stream.Rewind();

        media.SetValue(_mediaFileManager,
                       _mediaUrlGenerators,
                       _shortStringHelper,
                       _contentTypeBaseServiceProvider,
                       UmbracoConstants.Conventions.Media.File,
                       blob.Filename,
                       blob.Stream);

        _mediaService.Save(media);

        return media.Key;
    }

    private static string BuildValue(Guid mediaKey) {
        var item = new JObject {
            ["key"] = Guid.NewGuid().ToString(),
            ["mediaKey"] = mediaKey.ToString(),
            ["crops"] = new JArray(),
            ["focalPoint"] = null
        };

        return JsonConvert.SerializeObject(new JArray(item));
    }

    private static bool IsImage(string filename) {
        return ImageExtensions.Contains(Path.GetExtension(filename ?? string.Empty));
    }
}

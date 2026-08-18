using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Plugins.Extensions;
using N3O.Umbraco.Utilities;
using System.Collections.Generic;
using Umbraco.Cms.Core.IO;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters;

// Shared base for the native inline media editors (Umbraco.UploadField, Umbraco.ImageCropper). Unlike the
// media picker (which references a media-library node), these editors store the file inline on the property,
// so import writes the uploaded file straight to the media file system — the same /media/{id}/{filename}
// folder-per-item layout the editors use — and the subclass builds the editor's stored value shape.
public abstract class InlineMediaPropertyConverter : PropertyConverter<Blob, string> {
    private readonly IUrlBuilder _urlBuilder;
    private readonly MediaFileManager _mediaFileManager;
    private readonly ILocalClock _clock;

    protected InlineMediaPropertyConverter(IColumnRangeBuilder columnRangeBuilder,
                                           IUrlBuilder urlBuilder,
                                           MediaFileManager mediaFileManager,
                                           ILocalClock clock)
        : base(columnRangeBuilder) {
        _urlBuilder = urlBuilder;
        _mediaFileManager = mediaFileManager;
        _clock = clock;
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
               (alias, blob) => contentBuilder.Raw(alias).Set(BuildValue(StoreFile(blob))));
    }

    protected abstract string BuildValue(string urlPath);

    protected string AbsoluteUrl(string path) {
        return path.HasValue() ? _urlBuilder.Root().AppendPathSegment(path).ToString() : null;
    }

    private string StoreFile(Blob blob) {
        var instant = _clock.GetCurrentInstant();
        var storagePath = blob.Filename.GetStoragePath(instant);

        blob.Stream.Rewind();

        if (!_mediaFileManager.FileSystem.FileExists(storagePath)) {
            _mediaFileManager.FileSystem.AddFile(storagePath, blob.Stream, false);
        }

        return blob.Filename.GetMediaUrlPath(instant);
    }
}

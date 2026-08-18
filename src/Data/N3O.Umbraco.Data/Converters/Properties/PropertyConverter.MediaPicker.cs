using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using UmbracoConstants = Umbraco.Cms.Core.Constants;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters;

public class MediaPickerPropertyConverter : PropertyConverter<Blob, string> {
    private static readonly string EditorAlias = UmbracoPropertyEditors.Aliases.MediaPicker3;

    private readonly IContentHelper _contentHelper;
    private readonly IMediaUrl _mediaUrl;
    private readonly IMediaService _mediaService;
    private readonly MediaFileManager _mediaFileManager;
    private readonly MediaUrlGeneratorCollection _mediaUrlGenerators;
    private readonly IShortStringHelper _shortStringHelper;
    private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;
    private readonly ContentSettings _contentSettings;

    public MediaPickerPropertyConverter(IColumnRangeBuilder columnRangeBuilder,
                                        IContentHelper contentHelper,
                                        IMediaUrl mediaUrl,
                                        IMediaService mediaService,
                                        MediaFileManager mediaFileManager,
                                        MediaUrlGeneratorCollection mediaUrlGenerators,
                                        IShortStringHelper shortStringHelper,
                                        IContentTypeBaseServiceProvider contentTypeBaseServiceProvider,
                                        IOptions<ContentSettings> contentSettings)
        : base(columnRangeBuilder) {
        _contentHelper = contentHelper;
        _mediaUrl = mediaUrl;
        _mediaService = mediaService;
        _mediaFileManager = mediaFileManager;
        _mediaUrlGenerators = mediaUrlGenerators;
        _shortStringHelper = shortStringHelper;
        _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;
        _contentSettings = contentSettings.Value;
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
        ImportAll(errorLog,
                  propertyInfo,
                  fields,
                  s => parser.Blob.Parse(s, OurDataTypes.Blob.GetClrType()),
                  (alias, blobs) => contentBuilder.Raw(alias).Set(BuildValue(blobs.Select(CreateMedia))));
    }

    protected override int GetMaxValues(UmbracoPropertyInfo propertyInfo) {
        var configuration = propertyInfo.DataType.ConfigurationAs<MediaPicker3Configuration>();

        return configuration.Multiple ? DataConstants.Limits.Columns.MaxValues : 1;
    }

    private string GetUrl(MediaWithCrops media) {
        return media == null ? null : _mediaUrl.GetMediaUrl(media, urlMode: UrlMode.Absolute);
    }

    private Guid CreateMedia(Blob blob) {
        var key = DeterministicMediaKey(blob);
        var existing = _mediaService.GetById(key);

        if (existing != null) {
            return existing.Key;
        }

        var mediaTypeAlias = IsImage(blob.Filename)
                                 ? UmbracoConstants.Conventions.MediaTypes.Image
                                 : UmbracoConstants.Conventions.MediaTypes.File;

        var media = _mediaService.CreateMedia(blob.Filename, UmbracoConstants.System.Root, mediaTypeAlias);
        media.Key = key;

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

    private static Guid DeterministicMediaKey(Blob blob) {
        blob.Stream.Rewind();

        using (var sha = SHA256.Create()) {
            var hash = sha.ComputeHash(blob.Stream);

            blob.Stream.Rewind();

            return new Guid(hash[..16]);
        }
    }

    private static string BuildValue(IEnumerable<Guid> mediaKeys) {
        var items = new JArray();

        foreach (var mediaKey in mediaKeys) {
            items.Add(new JObject {
                ["key"] = Guid.NewGuid().ToString(),
                ["mediaKey"] = mediaKey.ToString(),
                ["crops"] = new JArray(),
                ["focalPoint"] = null
            });
        }

        return JsonConvert.SerializeObject(items);
    }

    private bool IsImage(string filename) {
        var extension = Path.GetExtension(filename ?? string.Empty).TrimStart('.');

        return _contentSettings.Imaging.ImageFileTypes.Any(x => x.EqualsInvariant(extension));
    }
}

using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters;

public class ImageCropperPropertyConverter : InlineMediaPropertyConverter {
    private static readonly string EditorAlias = UmbracoPropertyEditors.Aliases.ImageCropper;

    private readonly IContentHelper _contentHelper;

    public ImageCropperPropertyConverter(IColumnRangeBuilder columnRangeBuilder,
                                         IUrlBuilder urlBuilder,
                                         MediaFileManager mediaFileManager,
                                         ILocalClock clock,
                                         IContentHelper contentHelper)
        : base(columnRangeBuilder, urlBuilder, mediaFileManager, clock) {
        _contentHelper = contentHelper;
    }

    public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(EditorAlias);
    }

    protected override IEnumerable<Cell<string>> GetCells(IContentProperty contentProperty,
                                                          UmbracoPropertyInfo propertyInfo) {
        var value = _contentHelper.GetConvertedValue<ImageCropperValueConverter, ImageCropperValue>(contentProperty.ContentType.Alias,
                                                                                                    contentProperty.Type.Alias,
                                                                                                    contentProperty.Value);

        return OurDataTypes.String.Cell(AbsoluteUrl(value?.Src)).Yield();
    }

    protected override string BuildValue(string urlPath) {
        var value = new JObject {
            ["src"] = urlPath,
            ["crops"] = new JArray(),
            ["focalPoint"] = new JObject { ["left"] = 0.5, ["top"] = 0.5 }
        };

        return JsonConvert.SerializeObject(value);
    }
}

using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Uploader.Extensions;
using N3O.Umbraco.Utilities;
using System.Collections.Generic;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Uploader.Data.Converters;

public class UploaderPropertyConverter : PropertyConverter<Blob, string> {
    private readonly IContentHelper _contentHelper;
    private readonly IUrlBuilder _urlBuilder;

    public UploaderPropertyConverter(IColumnRangeBuilder columnRangeBuilder,
                                    IContentHelper contentHelper,
                                    IUrlBuilder urlBuilder)
        : base(columnRangeBuilder) {
        _contentHelper = contentHelper;
        _urlBuilder = urlBuilder;
    }

    public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(UploaderConstants.PropertyEditorAlias);
    }

    protected override IEnumerable<Cell<string>> GetCells(IContentProperty contentProperty,
                                                          UmbracoPropertyInfo propertyInfo) {
        var fileUpload = _contentHelper.GetFileUpload(contentProperty);

        return OurDataTypes.String
                           .Cell(fileUpload.IfNotNull(x => _urlBuilder.Root().AppendPathSegment(x.Src)))
                           .Yield();
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
               (alias, value) => value.IfNotNull(x => contentBuilder.Uploader(alias).SetFile(x)));
    }
}

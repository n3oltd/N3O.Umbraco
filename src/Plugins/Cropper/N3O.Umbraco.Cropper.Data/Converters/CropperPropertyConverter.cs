using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Cropper.Extensions;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System.Collections.Generic;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Cropper.Data.Converters {
    public class CropperPropertyConverter : PropertyConverter<Blob, string> {
        private readonly IContentHelper _contentHelper;
        private readonly IUrlBuilder _urlBuilder;

        public CropperPropertyConverter(IColumnRangeBuilder columnRangeBuilder,
                                        IContentHelper contentHelper,
                                        IUrlBuilder urlBuilder)
            : base(columnRangeBuilder) {
            _contentHelper = contentHelper;
            _urlBuilder = urlBuilder;
        }

        public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
            return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(CropperConstants.PropertyEditorAlias);
        }

        protected override IEnumerable<Cell<string>> GetCells(IContentProperty contentProperty,
                                                              UmbracoPropertyInfo propertyInfo) {
            var croppedImage = _contentHelper.GetCroppedImage(contentProperty);

            return OurDataTypes.String.Cell(croppedImage?.GetUncroppedUrl(_urlBuilder).AbsoluteUri).Yield();
        }

        public override void Import(IContentBuilder contentBuilder,
                                    IEnumerable<IPropertyConverter> converters,
                                    IParser parser,
                                    ErrorLog errorLog,
                                    string columnTitlePrefix,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<ImportField> fields) {
            var configuration = propertyInfo.DataType.ConfigurationAs<CropperConfiguration>();
            
            Import(errorLog,
                   propertyInfo,
                   fields,
                   s => parser.Blob.Parse(s, OurDataTypes.Blob.GetClrType()),
                   (alias, value) => value.IfNotNull(x => contentBuilder.Cropper(alias)
                                                                        .SetImage(x)
                                                                        .AutoCrop(configuration)));
        }
    }
}
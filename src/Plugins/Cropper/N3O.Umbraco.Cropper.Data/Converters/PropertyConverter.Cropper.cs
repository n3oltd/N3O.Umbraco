using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Cropper.Extensions;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cropper.Data.Converters {
    public class CropperPropertyConverter : PropertyConverter {
        public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
            return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(CropperConstants.PropertyEditorAlias);
        }

        public override IEnumerable<Cell> Export(ContentProperties content, UmbracoPropertyInfo propertyInfo) {
            throw new NotImplementedException();
        }

        public override void Import(IContentBuilder contentBuilder,
                                    IParser parser,
                                    ErrorLog errorLog,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<string> source) {
            var configurations = (CropperConfiguration) propertyInfo.DataType.Configuration;
            
            Import(errorLog,
                   propertyInfo,
                   source,
                   s => parser.Blob.Parse(s, Umbraco.Data.Lookups.DataTypes.Blob.GetClrType()),
                   (alias, value) => contentBuilder.Cropper(alias).SetImage(value).AutoCrop(configurations));
        }
    }
}
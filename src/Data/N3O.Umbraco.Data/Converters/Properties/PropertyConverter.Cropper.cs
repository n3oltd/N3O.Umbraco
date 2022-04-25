using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Converters {
    public class CropperPropertyConverter : PropertyConverter {
        public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
            return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(CropperConstants.PropertyEditorAlias);
        }

        public override IEnumerable<Cell> Export(ContentProperties content, UmbracoPropertyInfo propertyInfo) {
            throw new NotImplementedException();
        }

        public override void Import(IContentBuilder contentBuilder,
                                    IParser parser,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<string> source) {
            throw new NotImplementedException();
        }
    }
}
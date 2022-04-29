using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Konstrukt;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.PropertyEditors;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters {
    public class NestedContentPropertyConverter : PropertyConverter {
        public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
            return propertyInfo.Type
                               .PropertyEditorAlias
                               .EqualsInvariant(UmbracoPropertyEditors.Aliases.NestedContent);
        }

        public override IEnumerable<Cell> Export(ContentProperties content, UmbracoPropertyInfo propertyInfo) {
            throw new NotImplementedException();
        }

        public override void Import(IContentBuilder contentBuilder,
                                    IParser parser,
                                    ErrorLog errorLog,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<Field> source) {
            throw new NotImplementedException();
        }
        
        protected override int GetMaxValues(UmbracoPropertyInfo propertyInfo) {
            var configuration = (NestedContentConfiguration) propertyInfo.DataType.Configuration;

            return configuration.MaxItems ?? 0;
        }
    }
}
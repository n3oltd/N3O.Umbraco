using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters {
    public class TextBoxPropertyConverter : PropertyConverter {
        public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
            return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(UmbracoPropertyEditors.Aliases.TextBox);
        }

        public override IEnumerable<Cell> Export(ContentProperties content, UmbracoPropertyInfo propertyInfo) {
            return ExportValue<string>(content, propertyInfo, x => DataTypes.String.Cell(x));
        }

        public override void Import(IContentBuilder contentBuilder,
                                    IParser parser,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<string> source) {
            throw new NotImplementedException();
            // Import(propertyInfo,
            //        values,
            //        s => parser.String.Parse(s, typeof(string)),
            //        (alias, value) => contentBuilder.TextBox(alias).Set(value));
        }
    }
}
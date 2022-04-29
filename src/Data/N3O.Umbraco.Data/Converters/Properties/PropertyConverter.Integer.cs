using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters {
    public class IntegerPropertyConverter : PropertyConverter {
        public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
            return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(UmbracoPropertyEditors.Aliases.Integer);
        }

        public override IEnumerable<Cell> Export(ContentProperties content, UmbracoPropertyInfo propertyInfo) {
            return ExportValue<int>(content,
                                    propertyInfo,
                                    x => DataTypes.Integer.Cell(x));
        }

        public override void Import(IContentBuilder contentBuilder,
                                    IParser parser,
                                    ErrorLog errorLog,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<ImportField> fields) {
            Import(errorLog,
                   propertyInfo,
                   fields,
                   s => parser.Integer.Parse(s, DataTypes.Integer.GetClrType()),
                   (alias, value) => contentBuilder.Numeric(alias).SetInteger(value));
        }
    }
}
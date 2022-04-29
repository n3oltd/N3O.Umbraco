using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using NodaTime.Extensions;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.PropertyEditors;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters {
    public class DateTimePropertyConverter : PropertyConverter {
        public DateTimePropertyConverter(IColumnRangeBuilder columnRangeBuilder) : base(columnRangeBuilder) { }
        
        public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
            return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(UmbracoPropertyEditors.Aliases.DateTime);
        }

        public override IReadOnlyList<Cell> Export(ContentProperties content, UmbracoPropertyInfo propertyInfo) {
            return ExportValue<DateTime?>(content,
                                          propertyInfo,
                                          x => x == null ? null : DataTypes.DateTime.Cell(x.Value.ToLocalDateTime()));
        }

        public override void Import(IContentBuilder contentBuilder,
                                    IParser parser,
                                    ErrorLog errorLog,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<ImportField> fields) {
            var configuration = (DateTimeConfiguration) propertyInfo.DataType.Configuration;

            // h or H in format indicates includes some component of time
            if (configuration.Format.Contains("h", StringComparison.InvariantCultureIgnoreCase)) {
                Import(errorLog,
                       propertyInfo,
                       fields,
                       s => parser.DateTime.Parse(s, DataTypes.DateTime.GetClrType()),
                       (alias, value) => contentBuilder.DateTime(alias).SetDateTime(value));
            } else {
                Import(errorLog,
                       propertyInfo,
                       fields,
                       s => parser.Date.Parse(s, DataTypes.Date.GetClrType()),
                       (alias, value) => contentBuilder.DateTime(alias).SetDateTime(value?.AtMidnight()));
            }
        }
    }
}
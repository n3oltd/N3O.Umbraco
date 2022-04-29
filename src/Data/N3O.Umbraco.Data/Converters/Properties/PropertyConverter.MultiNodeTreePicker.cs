using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Converters {
    public class MultiNodeTreePickerPropertyConverter : PropertyConverter {
        public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
            return propertyInfo.Type
                               .PropertyEditorAlias
                               .EqualsInvariant(UmbracoPropertyEditors.Aliases.MultiNodeTreePicker);
        }

        public override IEnumerable<Cell> Export(ContentProperties content, UmbracoPropertyInfo propertyInfo) {
            throw new NotImplementedException();
        }

        public override void Import(IContentBuilder contentBuilder,
                                    IParser parser,
                                    ErrorLog errorLog,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<ImportField> fields) {
            ImportAll(errorLog,
                      propertyInfo,
                      fields,
                      s => Parse(parser, propertyInfo, s),
                      (alias, values) => contentBuilder.ContentPicker(alias).SetContent(values));
        }

        protected override int GetMaxValues(UmbracoPropertyInfo propertyInfo) {
            var configuration = (MultiNodePickerConfiguration) propertyInfo.DataType.Configuration;

            return configuration.MaxNumber;
        }
        
        private ParseResult<IPublishedContent> Parse(IParser parser, UmbracoPropertyInfo propertyInfo, string source) {
            var configuration = (MultiNodePickerConfiguration) propertyInfo.DataType.Configuration;
            var parentId = configuration.TreeSource?.StartNodeId?.ToId();

            return parser.PublishedContent.Parse(source, DataTypes.PublishedContent.GetClrType(), parentId);
        }
    }
}
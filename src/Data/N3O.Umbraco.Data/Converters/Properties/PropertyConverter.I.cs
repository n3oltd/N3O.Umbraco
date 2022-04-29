using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Parsing;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Converters {
    public interface IPropertyConverter {
        IEnumerable<Cell> Export(ContentProperties contentProperties, UmbracoPropertyInfo propertyInfo);
        TemplateColumn GetTemplateColumn(UmbracoPropertyInfo propertyInfo);
        void Import(IContentBuilder contentBuilder,
                    IParser parser,
                    ErrorLog errorLog,
                    UmbracoPropertyInfo propertyInfo,
                    IEnumerable<ImportField> fields);
        bool IsConverter(UmbracoPropertyInfo propertyInfo);
    }
}
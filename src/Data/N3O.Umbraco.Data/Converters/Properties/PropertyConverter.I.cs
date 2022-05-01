using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Parsing;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Converters {
    public interface IPropertyConverter {
        void Export(IUntypedTableBuilder tableBuilder,
                    IEnumerable<IPropertyConverter> converters,
                    string columnTitlePrefix,
                    IContentProperty contentProperty,
                    UmbracoPropertyInfo propertyInfo);
        
        IReadOnlyList<Column> GetColumns(IEnumerable<IPropertyConverter> converters,
                                         UmbracoPropertyInfo propertyInfo,
                                         string columnTitlePrefix);
        
        void Import(IContentBuilder contentBuilder,
                    IEnumerable<IPropertyConverter> converters,
                    IParser parser,
                    ErrorLog errorLog,
                    string columnTitlePrefix,
                    UmbracoPropertyInfo propertyInfo,
                    IEnumerable<ImportField> fields);
        
        bool IsConverter(UmbracoPropertyInfo propertyInfo);
    }
}
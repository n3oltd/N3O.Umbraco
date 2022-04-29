using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Converters {
    public abstract class PropertyConverter : IPropertyConverter {
        public abstract IEnumerable<Cell> Export(ContentProperties content, UmbracoPropertyInfo propertyInfo);
        
        public virtual TemplateColumn GetTemplateColumn(UmbracoPropertyInfo propertyInfo) {
            return new TemplateColumn(propertyInfo.GetName(), GetMaxValues(propertyInfo), propertyInfo);
        }

        protected IEnumerable<Cell> ExportValue<T>(ContentProperties contentProperties,
                                                   UmbracoPropertyInfo propertyInfo,
                                                   Func<T, Cell> toCell) {
            var contentProperty = contentProperties.Properties
                                                   .Single(x => x.Alias.EqualsInvariant(propertyInfo.Type.Alias));
            var value = contentProperty.Value;

            if (value == null) {
                return null;
            }

            return toCell((T) value).Yield();
        }

        public abstract void Import(IContentBuilder contentBuilder,
                                    IParser parser,
                                    ErrorLog errorLog,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<ImportField> fields);

        public abstract bool IsConverter(UmbracoPropertyInfo propertyInfo);
        
        protected virtual int GetMaxValues(UmbracoPropertyInfo propertyInfo) => 1;
        
        protected void Import<T>(ErrorLog errorLog,
                                 UmbracoPropertyInfo propertyInfo,
                                 IEnumerable<ImportField> fields,
                                 Func<string, ParseResult<T>> parse,
                                 Action<string, T> setContent) {
            if (fields.OrEmpty().Count() > 1) {
                throw new Exception($"Multiple values passed to {nameof(Import)}");
            }
            
            ImportAll(errorLog, propertyInfo, fields, parse, (alias, values) => setContent(alias, values.Single()));
        }
        
        protected void ImportAll<T>(ErrorLog errorLog,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<ImportField> fields,
                                    Func<string, ParseResult<T>> parse,
                                    Action<string, IEnumerable<T>> setContent) {
            var values = new List<T>();

            foreach (var field in fields) {
                var parseResult = parse(field.Value);

                if (parseResult.Success) {
                    values.Add(parseResult.Value);    
                } else {
                    errorLog.AddError<Strings>(s => s.ParsingFailed_1, field.Value, field.Name);
                }
            }

            if (values.HasAny()) {
                setContent(propertyInfo.Type.Alias, values);
            }
        }

        public class Strings : CodeStrings {
            public string ParsingFailed_1 => $"The value {"{0}".Quote()} is invalid for {"{1}".Quote()}";
        }
    }
}
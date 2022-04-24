using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
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
            var value = contentProperty?.Value;

            if (value == null) {
                return null;
            }

            return toCell((T) value).Yield();
        }

        public abstract void Import(IContentBuilder contentBuilder,
                                    IParser parser,
                                    UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<string> values);

        public abstract bool IsConverter(UmbracoPropertyInfo propertyInfo);
        
        protected virtual int GetMaxValues(UmbracoPropertyInfo propertyInfo) => 1;
        
        protected void Import<T>(UmbracoPropertyInfo propertyInfo,
                                 IEnumerable<string> strValues,
                                 Func<string, ParseResult<T>> parse,
                                 Action<string, T> setContent) {
            if (strValues.OrEmpty().Count() > 1) {
                throw new Exception($"Multiple values passed to {nameof(Import)}");
            }
            
            ImportAll(propertyInfo, strValues, parse, (alias, values) => setContent(alias, values.Single()));
        }
        
        protected void ImportAll<T>(UmbracoPropertyInfo propertyInfo,
                                    IEnumerable<string> strValues,
                                    Func<string, ParseResult<T>> parse,
                                    Action<string, IEnumerable<T>> setContent) {
            var values = new List<T>();

            foreach (var strValue in strValues) {
                var parseResult = parse(strValue);

                if (!parseResult.Success) {
                    // TODO Replace with appropriate exception that indicates value that failed (strValue)
                    throw new Exception();
                }
                
                values.Add(parseResult.Value);
            }
            
            setContent(propertyInfo.ContentType.Alias, values);
        }
    }
}
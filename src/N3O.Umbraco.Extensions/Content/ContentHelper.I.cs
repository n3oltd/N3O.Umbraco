using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Content {
    public interface IContentHelper {
        IReadOnlyList<T> Ancestor<T>(IContent content) where T : PublishedContentModel;
        
        IReadOnlyList<IContent> Ancestors(IContent content);
        
        IReadOnlyList<T> Children<T>(IContent content) where T : PublishedContentModel;
    
        IReadOnlyList<IContent> Children(IContent content);

        IReadOnlyList<T> Descendants<T>(IContent content) where T : PublishedContentModel;
    
        IReadOnlyList<IContent> Descendants(IContent content);
    
        TProperty GetCustomConverterValue<TConverter, TContent, TProperty>(IContent content,
                                                                           Expression<Func<TContent, TProperty>> memberLambda)
            where TConverter : class, IPropertyValueConverter;

        TProperty GetCustomConverterValue<TConverter, TProperty>(IContent content,
                                                                 IPublishedPropertyType publishedPropertyType,
                                                                 string propertyAlias)
            where TConverter : class, IPropertyValueConverter;

        IEnumerable<IPublishedElement> GetNestedContent(IContent content);
    
        IReadOnlyList<IPublishedElement> GetNestedContent(IContent content, IProperty property);

        IEnumerable<TProperty> GetMultiNestedContentValue<TContent, TProperty>(IContent content,
                                                                               Expression<Func<TContent, IEnumerable<TProperty>>> memberLambda);

        TProperty GetPickerValue<TContent, TProperty>(IContent content, Expression<Func<TContent, TProperty>> memberLambda);

        TProperty GetSingleNestedContentValue<TContent, TProperty>(IContent content,
                                                                   Expression<Func<TContent, TProperty>> memberLambda);
    }
}

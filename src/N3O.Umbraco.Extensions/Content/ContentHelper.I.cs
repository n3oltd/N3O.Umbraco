using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Content;

public interface IContentHelper {
    IReadOnlyList<IContent> GetAncestors(IContent content);
    
    IReadOnlyList<IContent> GetChildren(IContent content);

    ContentProperties GetContentProperties(IContent content);

    ContentProperties GetContentProperties(Guid contentId,
                                           string contentTypeAlias,
                                           IEnumerable<(IPropertyType Type, object Value)> properties);
    
    TProperty GetConvertedValue<TConverter, TProperty>(string contentTypeAlias,
                                                       string propertyTypeAlias,
                                                       object propertyValue)
        where TConverter : class, IPropertyValueConverter;
    
    IReadOnlyList<IContent> GetDescendants(IContent content);

    IReadOnlyList<T> GetPublishedAncestors<T>(IContent content) where T : IPublishedContent;
    
    IReadOnlyList<T> GetPublishedChildren<T>(IContent content) where T : IPublishedContent;
    
    IReadOnlyList<T> GetPublishedDescendants<T>(IContent content) where T : IPublishedContent;
}

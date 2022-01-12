using Perplex.ContentBlocks.Rendering;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Content {
    public interface IContentHelper {
        IReadOnlyList<IContent> GetAncestors(IContent content);
        
        IReadOnlyList<IContent> GetChildren(IContent content);
        
        ContentBlocks GetContentBlocks(string contentTypeAlias, string propertyTypeAlias, object propertyValue);
        
        ContentNode GetContentNode(IContent content);
        
        TProperty GetConvertedValue<TConverter, TProperty>(string contentTypeAlias,
                                                           string propertyTypeAlias,
                                                           object propertyValue)
            where TConverter : class, IPropertyValueConverter;
        
        IReadOnlyList<IContent> GetDescendants(IContent content);
        
        IPublishedElement GetNestedContent(string contentTypeAlias, string propertyTypeAlias, object propertyValue);
        
        IReadOnlyList<IPublishedElement> GetNestedContents(string contentTypeAlias,
                                                           string propertyTypeAlias,
                                                           object propertyValue);
        
        T GetPickerValue<T>(string contentTypeAlias, string propertyTypeAlias, object propertyValue);
        
        IReadOnlyList<T> GetPickerValues<T>(string contentTypeAlias, string propertyTypeAlias, object propertyValue);
        
        IReadOnlyList<T> GetPublishedAncestors<T>(IContent content) where T : IPublishedContent;
        
        IReadOnlyList<T> GetPublishedChildren<T>(IContent content) where T : IPublishedContent;
        
        IReadOnlyList<T> GetPublishedDescendants<T>(IContent content) where T : IPublishedContent;
    }
}

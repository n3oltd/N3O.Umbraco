using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace N3O.Umbraco.Content;

public class ContentHelper : IContentHelper {
    private readonly IServiceProvider _serviceProvider;
    private readonly IContentService _contentService;
    private readonly IPublishedModelFactory _publishedModelFactory;
    private readonly IPublishedContentTypeFactory _publishedContentTypeFactory;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly NestedContentManyValueConverter _nestedContentManyValueConverter;

    public ContentHelper(IServiceProvider serviceProvider,
                         IContentService contentService,
                         IPublishedModelFactory publishedModelFactory,
                         IPublishedContentTypeFactory publishedContentTypeFactory,
                         IUmbracoContextAccessor umbracoContextAccessor,
                         NestedContentManyValueConverter nestedContentManyValueConverter) {
        _serviceProvider = serviceProvider;
        _contentService = contentService;
        _publishedModelFactory = publishedModelFactory;
        _publishedContentTypeFactory = publishedContentTypeFactory;
        _umbracoContextAccessor = umbracoContextAccessor;
        _nestedContentManyValueConverter = nestedContentManyValueConverter;
    }
    
    public IReadOnlyList<IContent> Children<T>(IContent content) where T : IPublishedContent {
        return Children(content).Where(x => x.ContentType.Alias == AliasHelper.ForContentType<T>()).ToList();
    }

    public IReadOnlyList<IContent> Children(IContent content) {
        var children = _contentService.GetPagedChildren(content.Id, 0, 1000, out _).ToList();

        return children;
    }

    public IReadOnlyList<IContent> Descendants<T>(IContent content) where T : IPublishedContent {
        return Descendants(content).Where(x => x.ContentType.Alias == AliasHelper.ForContentType<T>()).ToList();
    }

    public IReadOnlyList<IContent> Descendants(IContent content) {
        var descendants = _contentService.GetPagedDescendants(content.Id, 0, 1000, out _).ToList();

        return descendants;
    }

    public TProperty GetCustomConverterValue<TConverter, TContent, TProperty>(IContent content,
                                                                              Expression<Func<TContent, TProperty>> memberLambda)
        where TConverter : class, IPropertyValueConverter {
        _umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext);
        
        var propertyAlias = AliasHelper.ForProperty(memberLambda);
        var publishedContentType = umbracoContext.PublishedSnapshot.Content.GetContentType(content.ContentType.Alias);
        var publishedPropertyType = publishedContentType.GetPropertyType(propertyAlias);

        return GetCustomConverterValue<TConverter, TProperty>(content, publishedPropertyType, propertyAlias);
    }
    
    public TProperty GetCustomConverterValue<TConverter, TProperty>(IContent content,
                                                                    IPublishedPropertyType publishedPropertyType,
                                                                    string propertyAlias)
        where TConverter : class, IPropertyValueConverter {
        var converter = _serviceProvider.GetRequiredService<TConverter>();
        var source = content.GetValue<object>(propertyAlias);

        if (source == null) {
            return default;
        }

        var intermediate = converter.ConvertSourceToIntermediate(null, publishedPropertyType, source, false);
        var result = (TProperty) converter.ConvertIntermediateToObject(null,
                                                                       publishedPropertyType,
                                                                       PropertyCacheLevel.None,
                                                                       intermediate,
                                                                       false);

        return result;
    }
    
    public IEnumerable<IPublishedElement> GetNestedContent(IContent content) {
        return content.Properties
                      .Where(p => p.PropertyType.IsNestedContent())
                      .SelectMany(p => GetNestedContent(content, p));
    }

    public IReadOnlyList<IPublishedElement> GetNestedContent(IContent content, IProperty property) {
        _umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext);
        
        var publishedContentType =  umbracoContext.PublishedSnapshot.Content.GetContentType(content.ContentType.Alias);
        var publishedPropertyType = new PublishedPropertyType(publishedContentType,
                                                              content.Properties[property.Alias].PropertyType,
                                                              new PropertyValueConverterCollection(() => _nestedContentManyValueConverter.Yield()),
                                                              _publishedModelFactory,
                                                              _publishedContentTypeFactory);

        var nestedContent = GetCustomConverterValue<NestedContentManyValueConverter, IEnumerable<IPublishedElement>>(content,
                                                                                                                     publishedPropertyType,
                                                                                                                     property.Alias).ToList();

        return nestedContent;
    }

    public IEnumerable<TProperty> GetMultiNestedContentValue<TContent, TProperty>(IContent content,
                                                                                  Expression<Func<TContent, IEnumerable<TProperty>>> memberLambda) {
        return GetCustomConverterValue<NestedContentManyValueConverter, TContent, IEnumerable<TProperty>>(content, memberLambda)?.ToList()
               ?? Enumerable.Empty<TProperty>();
    }
    
    public TProperty GetPickerValue<TContent, TProperty>(IContent content, Expression<Func<TContent, TProperty>> memberLambda) {
        return GetCustomConverterValue<MultiNodeTreePickerValueConverter, TContent, TProperty>(content, memberLambda);
    }
    
    public TProperty GetSingleNestedContentValue<TContent, TProperty>(IContent content,
                                                                      Expression<Func<TContent, TProperty>> memberLambda) {
        return GetCustomConverterValue<NestedContentSingleValueConverter, TContent, TProperty>(content, memberLambda);
    }
}

using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.ValueConverters;
using Newtonsoft.Json;
using Perplex.ContentBlocks.PropertyEditor;
using Perplex.ContentBlocks.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Persistence.Querying;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Content {
    public class ContentHelper : IContentHelper {
        private readonly Lazy<IServiceProvider> _serviceProvider;
        private readonly Lazy<IContentService> _contentService;
        private readonly Lazy<IContentTypeService> _contentTypeService;
        private readonly Lazy<IContentLocator> _contentLocator;
        private readonly Lazy<IUmbracoContextFactory> _umbracoContextFactory;

        public ContentHelper(Lazy<IServiceProvider> serviceProvider,
                             Lazy<IContentService> contentService,
                             Lazy<IContentTypeService> contentTypeService,
                             Lazy<IContentLocator> contentLocator,
                             Lazy<IUmbracoContextFactory> umbracoContextFactory) {
            _serviceProvider = serviceProvider;
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _contentLocator = contentLocator;
            _umbracoContextFactory = umbracoContextFactory;
        }

        public IReadOnlyList<IContent> GetAncestors(IContent content) {
            var list = new List<IContent>();

            while (content.ParentId != -1) {
                content = _contentService.Value.GetById(content.ParentId);

                list.Add(content);
            }

            return list;
        }

        public IReadOnlyList<IContent> GetChildren(IContent content) {
            return GetAllPagedContent(content, _contentService.Value.GetPagedChildren);
        }

        public ContentBlocks GetContentBlocks(string contentTypeAlias, string propertyTypeAlias, object propertyValue) {
            var contentBlocks = GetConvertedValue<ContentBlocksValueConverter, ContentBlocks>(contentTypeAlias,
                                                                                              propertyTypeAlias,
                                                                                              propertyValue);

            return contentBlocks;
        }

        public ContentNode GetContentNode(IContent content) {
            var properties = new List<ContentProperty>();
            var descendants = new List<ContentNode>();
            var contentType = _contentTypeService.Value.Get(content.ContentType.Alias);

            foreach (var property in content.Properties) {
                if (property.PropertyType.IsNestedContent()) {
                    var json = (string) property.GetValue();
                    var nestedContent = JsonConvert.DeserializeObject(json);
                    
                    descendants.AddRange(GetContentNodesForNestedContent(nestedContent));
                } else if (property.PropertyType.IsContentBlocks()) {
                    var json = (string) property.GetValue();
                    var blockContent = JsonConvert.DeserializeObject(json);

                    descendants.AddRange(GetContentNodesForBlockContent(blockContent));
                } else {
                    properties.Add(new ContentProperty(contentType, property.PropertyType, property.GetValue()));
                }
            }

            var contentNode = new ContentNode(content.Key, content.ContentType.Alias, properties, descendants);

            return contentNode;
        }

        public TProperty GetConvertedValue<TConverter, TProperty>(string contentTypeAlias,
                                                                  string propertyTypeAlias,
                                                                  object propertyValue)
            where TConverter : class, IPropertyValueConverter {
            var umbracoContext = _umbracoContextFactory.Value.EnsureUmbracoContext().UmbracoContext;
            var converter = _serviceProvider.Value.GetRequiredService<TConverter>();
            var publishedContentType = umbracoContext.PublishedSnapshot.Content.GetContentType(contentTypeAlias);
            var publishedPropertyType = publishedContentType.GetPropertyType(propertyTypeAlias);
            
            var source = propertyValue;

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
        
        public IReadOnlyList<IContent> GetDescendants(IContent content) {
            return GetAllPagedContent(content, _contentService.Value.GetPagedDescendants);
        }
        
        public IPublishedElement GetNestedContent(string contentTypeAlias,
                                                  string propertyTypeAlias,
                                                  object propertyValue) {
            return GetNestedContents(contentTypeAlias, propertyTypeAlias, propertyValue).Single();
        }
        
        public IReadOnlyList<IPublishedElement> GetNestedContents(string contentTypeAlias,
                                                                  string propertyTypeAlias,
                                                                  object propertyValue) {
            var publishedElements = GetConvertedValue<NestedContentManyValueConverter, IEnumerable<IPublishedElement>>(contentTypeAlias,
                                                                                                                       propertyTypeAlias,
                                                                                                                       propertyValue);

            return publishedElements.ToList();
        }

        public T GetPickerValue<T>(string contentTypeAlias,
                                   string propertyTypeAlias,
                                   object propertyValue) {
            return GetPickerValues<T>(contentTypeAlias, propertyTypeAlias, propertyValue).Single();
        }

        public IReadOnlyList<T> GetPickerValues<T>(string contentTypeAlias,
                                                   string propertyTypeAlias,
                                                   object propertyValue) {
            var items = GetConvertedValue<StronglyTypedMultiNodeTreePickerValueConverter, IEnumerable<T>>(contentTypeAlias,
                                                                                                          propertyTypeAlias,
                                                                                                          propertyValue);

            return items.ToList();
        }

        public IReadOnlyList<T> GetPublishedAncestors<T>(IContent content) where T : IPublishedContent {
            return GetAncestors(content).Select(x => _contentLocator.Value.ById<T>(x.Key)).ToList();
        }
        
        public IReadOnlyList<T> GetPublishedChildren<T>(IContent content) where T : IPublishedContent {
            return GetChildren(content).Select(x => _contentLocator.Value.ById<T>(x.Key)).ToList();
        }
        
        public IReadOnlyList<T> GetPublishedDescendants<T>(IContent content) where T : IPublishedContent {
            return GetDescendants(content).Select(x => _contentLocator.Value.ById<T>(x.Key)).ToList();
        }
        
        private IReadOnlyList<IContent> GetAllPagedContent(IContent content, GetPagedContent getPagedContent) {
            var descendants = new List<IContent>();

            var startIndex = 0;
            var pageSize = 100;

            while (true) {
                descendants.AddRange(getPagedContent(content.Id, startIndex, pageSize, out var totalRecords));

                if ((startIndex + pageSize) <= totalRecords) {
                    break;
                }

                startIndex += pageSize;
            }

            return descendants;
        }

        private IReadOnlyList<ContentNode> GetContentNodesForBlockContent(dynamic blockContent) {
            return GetContentNodesForNestedContent(blockContent.blocks.content);
        }

        private IReadOnlyList<ContentNode> GetContentNodesForNestedContent(dynamic nestedContent) {
            var contentNodes = new List<ContentNode>();

            foreach (var content in nestedContent) {
                var id = Guid.Parse((string) content.key);
                var contentTypeAlias = (string) content.ncContentTypeAlias;
                var contentType = _contentTypeService.Value.Get(contentTypeAlias);

                var properties = new List<ContentProperty>();
                var descendants = new List<ContentNode>();

                foreach (var propertyGroup in contentType.PropertyGroups) {
                    foreach (var propertyType in propertyGroup.PropertyTypes) {
                        var propertyValue = content[propertyType.Alias];

                        if (propertyType.IsNestedContent()) {
                            descendants.AddRange(GetContentNodesForNestedContent(propertyValue));
                        } else if (propertyType.IsContentBlocks()) {
                            descendants.AddRange(GetContentNodesForBlockContent(propertyValue));
                        } else {
                            properties.Add(new ContentProperty(contentType, propertyType, propertyValue));
                        }
                    }
                }

                contentNodes.Add(new ContentNode(id, contentTypeAlias, properties, descendants));
            }

            return contentNodes;
        }

        private delegate IEnumerable<IContent> GetPagedContent(int id,
                                                               long pageIndex,
                                                               int pageSize,
                                                               out long totalRecords,
                                                               IQuery<IContent> filter = null,
                                                               Ordering ordering = null);
    }
}

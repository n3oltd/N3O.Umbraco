using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;

namespace N3O.Umbraco.Content {
    public class PublishedContentHelper : IPublishedContentHelper {
        private readonly IContentService _contentService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IJsonProvider _jsonProvider;

        public PublishedContentHelper(IContentService contentService,
                                      UmbracoHelper umbracoHelper,
                                      IJsonProvider jsonProvider) {
            _contentService = contentService;
            _umbracoHelper = umbracoHelper;
            _jsonProvider = jsonProvider;
        }
    
        public T GetOrCreateFolder<T>(IPublishedContent content, string name)
            where T : PublishedContentModel {
            var publishedFolder = content.Child<T>(x => x.Name.EqualsInvariant(name));

            if (publishedFolder == null) {
                var contentFolder = _contentService.Create<T>(name, content.Id);

                _contentService.SaveAndPublish(contentFolder);

                publishedFolder = _umbracoHelper.Content(contentFolder.Id) as T;
            }

            return publishedFolder;
        }

        public void SortChildren<T>(IPublishedContent content, Func<T, object> keySelector, int userId = 0)
            where T : class, IPublishedContent {
            var sortOrder = 0;

            var children = content.Children<T>()
                                  .OrderBy(x => x.ContentType.Alias.Contains("Folder") ? 0 : 1)
                                  .ThenBy(keySelector)
                                  .Select(x => _contentService.GetById(x.Id))
                                  .ToList();

            foreach (var child in children) {
                child.SortOrder = sortOrder++;

                _contentService.SaveAndPublish(child);
            }
        }
        
        public void SortChildrenByName(IPublishedContent content) {
            SortChildren<IPublishedContent>(content, x => x.Name);
        }

        public JObject ToJObject(IPublishedContent content) {
            var jObject = new JObject();
            
            jObject.Add(nameof(IPublishedContent.Id), new JValue(content.Id));
            jObject.Add(nameof(IPublishedContent.Key), new JValue(content.Key));
            jObject.Add(nameof(IPublishedContent.ContentType), new JValue(content.ContentType.Alias));
            jObject.Add(nameof(IPublishedContent.Name), new JValue(content.Name));
            jObject.Add(nameof(IPublishedContent.CreateDate), new JValue(content.CreateDate));
            jObject.Add(nameof(IPublishedContent.UpdateDate), new JValue(content.UpdateDate));

            foreach (var property in content.Properties) {
                var jToken = property.GetValue().ToJToken(_jsonProvider);
                
                jObject.Add(property.Alias, jToken);
            }

            return jObject;
        }
    }
}

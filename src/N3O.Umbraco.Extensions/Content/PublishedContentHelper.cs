using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common;

namespace N3O.Umbraco.Content {
    public class PublishedContentHelper : IPublishedContentHelper {
        private readonly IContentService _contentService;
        private readonly UmbracoHelper _umbracoHelper;

        public PublishedContentHelper(IContentService contentService, UmbracoHelper umbracoHelper) {
            _contentService = contentService;
            _umbracoHelper = umbracoHelper;
        }
    
        public T GetOrCreateFolder<T>(IPublishedContent content, string name)
            where T : IPublishedContent {
            var publishedFolder = content.Child<T>(x => x.Name.EqualsInvariant(name));

            if (publishedFolder == null) {
                var contentFolder = _contentService.Create<T>(name, content.Id);

                _contentService.SaveAndPublish(contentFolder);

                publishedFolder = (T) _umbracoHelper.Content(contentFolder.Id);
            }

            return publishedFolder;
        }

        public void SortChildren<T>(IPublishedContent content, Func<T, object> keySelector)
            where T : IPublishedContent {
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
    }
}

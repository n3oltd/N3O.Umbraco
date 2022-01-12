using N3O.Umbraco.Content;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common;

namespace N3O.Umbraco.Context {
    public class CurrentPageAccessor : ICurrentPageAccessor {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IContentLocator _contentLocator;

        public CurrentPageAccessor(UmbracoHelper umbracoHelper, IContentLocator contentLocator) {
            _umbracoHelper = umbracoHelper;
            _contentLocator = contentLocator;
        }

        public IPublishedContent GetCurrentPage() {
            try {
                return _contentLocator.ById<IPublishedContent>(_umbracoHelper.AssignedContentItem?.Id ?? 0);
            } catch {
                return null;
            }
        }

        public T GetCurrentPage<T>() where T : IPublishedContent {
            return (T) GetCurrentPage();
        }
    }
}

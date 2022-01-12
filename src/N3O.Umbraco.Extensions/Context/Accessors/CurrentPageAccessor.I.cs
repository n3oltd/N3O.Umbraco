using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Context {
    public interface ICurrentPageAccessor {
        IPublishedContent GetCurrentPage();
        T GetCurrentPage<T>() where T : IPublishedContent;
    }
}

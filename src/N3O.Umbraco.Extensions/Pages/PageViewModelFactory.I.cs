using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages {
    public interface IPageViewModelFactory {
        IPageViewModel Create(IPublishedContent content, PageModulesData modulesData);
    }

    public interface IPageViewModelFactory<TContent> : IPageViewModelFactory where TContent : IPublishedContent {
        IPageViewModel<TContent> Create(TContent content, PageModulesData modulesData);
    }
}
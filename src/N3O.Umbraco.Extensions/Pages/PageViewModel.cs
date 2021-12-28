using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages;

public interface IPageViewModel : IContentModel {
    PageExtensionData ExtensionData { get; }
}

public interface IPageViewModel<TPage> : IPageViewModel where TPage : IPublishedContent {
    new TPage Content { get; }
}

public class PageViewModel<TPage> : ContentModel<TPage>, IPageViewModel<TPage> where TPage : IPublishedContent {
    public PageViewModel(PageParameters<TPage> parameters) : base(parameters.Content) {
        ExtensionData = parameters.ExtensionData;
    }

    public PageExtensionData ExtensionData { get; }
}

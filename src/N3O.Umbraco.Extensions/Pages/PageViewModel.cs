using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages;

public interface IPageViewModel : IContentModel {
    PageModulesData ModulesData { get; }
}

public interface IPageViewModel<out TPage> : IPageViewModel where TPage : IPublishedContent {
    new TPage Content { get; }
}

public class PageViewModel<TPage> : ContentModel<TPage>, IPageViewModel<TPage> where TPage : IPublishedContent {
    private readonly Func<string, string> _getText;

    public PageViewModel(PageParameters<TPage> parameters) : base(parameters.Content) {
        ModulesData = parameters.ModulesData;

        _getText = parameters.GetText;
    }

    public PageModulesData ModulesData { get; }

    public string GetText(string s) => _getText(s);
}

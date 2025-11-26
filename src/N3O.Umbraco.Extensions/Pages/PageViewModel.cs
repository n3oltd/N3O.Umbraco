using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages;

public interface IPageViewModel : IContentModel {
    PageModulesData ModulesData { get; }
    
    string GenerateId(params object[] contextValues);
    string GetText(string s);
}

public interface IPageViewModel<out TPage> : IPageViewModel where TPage : IPublishedContent {
    new TPage Content { get; }
}

public class PageViewModel<TPage> : ContentModel<TPage>, IPageViewModel<TPage> where TPage : IPublishedContent {
    private readonly Func<object[], string> _generateId;
    private readonly Func<string, string> _getText;

    public PageViewModel(PageParameters<TPage> parameters) : base(parameters.Content) {
        ModulesData = parameters.ModulesData;

        _generateId = parameters.GenerateId;
        _getText = parameters.GetText;
    }

    public PageModulesData ModulesData { get; }

    public string GenerateId(params object[] contextValues) => _generateId(contextValues.OrEmpty().Concat(Content.Key).ToArray());
    public string GetText(string s) => _getText(s);
}

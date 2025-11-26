using N3O.Umbraco.Utilities;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages;

public class PageParameters<TPage> where TPage : IPublishedContent {
    private readonly HtmlIds _htmlIds = new();
    
    public PageParameters(Func<string, string> getText,
                          TPage content,
                          PageModulesData modulesData) {
        GenerateId = x => _htmlIds.GenerateId(x);
        GetText = getText;
        Content = content;
        ModulesData = modulesData;
    }

    public Func<object[], string> GenerateId { get; }
    public Func<string, string> GetText { get; }
    public TPage Content { get; }
    public PageModulesData ModulesData { get; }
}

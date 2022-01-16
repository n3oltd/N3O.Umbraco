using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages {
    public class PageParameters<TPage> where TPage : IPublishedContent {
        public PageParameters(Func<string, string> getText, TPage content, PageModulesData modulesData) {
            GetText = getText;
            Content = content;
            ModulesData = modulesData;
        }

        public Func<string, string> GetText { get; }
        public TPage Content { get; }
        public PageModulesData ModulesData { get; }
    }
}
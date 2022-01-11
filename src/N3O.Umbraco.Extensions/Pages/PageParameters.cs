using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages {
    public class PageParameters<TPage> where TPage : IPublishedContent {
        public PageParameters(Func<string, string> getText, TPage content, PageModuleData moduleData) {
            GetText = getText;
            Content = content;
            ModuleData = moduleData;
        }

        public Func<string, string> GetText { get; }
        public TPage Content { get; }
        public PageModuleData ModuleData { get; }
    }
}
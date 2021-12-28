using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages {
    public class PageParameters<TPage> where TPage : IPublishedContent {
        public PageParameters(TPage content, PageExtensionData extensionData) {
            Content = content;
            ExtensionData = extensionData;
        }

        public TPage Content { get; }
        public PageExtensionData ExtensionData { get; }
    }
}
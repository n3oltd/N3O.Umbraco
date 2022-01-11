using Perplex.ContentBlocks.Rendering;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages {
    public interface IPageViewModel : IContentModel {
        PageModuleData ModuleData { get; }
        IContentBlocks Blocks { get; }
    }

    public interface IPageViewModel<out TPage> : IPageViewModel where TPage : IPublishedContent {
        new TPage Content { get; }
    }

    public class PageViewModel<TPage> : ContentModel<TPage>, IPageViewModel<TPage> where TPage : IPublishedContent {
        private readonly Func<string, string> _getText;

        public PageViewModel(PageParameters<TPage> parameters) : base(parameters.Content) {
            ModuleData = parameters.ModuleData;

            _getText = parameters.GetText;
        }

        public IContentBlocks Blocks => Content.GetProperty("blocks")?.GetValue() as IContentBlocks;
        public PageModuleData ModuleData { get; }

        public string GetText(string s) => _getText(s);
    }
}

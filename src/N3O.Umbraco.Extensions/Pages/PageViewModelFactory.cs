using System;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages {
    public class PageViewModelFactory<TPage, TViewModel> : IPageViewModelFactory<TPage>
        where TPage : class, IPublishedContent
        where TViewModel : IPageViewModel<TPage> {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<IServiceProvider, PageParameters<TPage>, TViewModel> _constructViewModel;

        public PageViewModelFactory(IServiceProvider serviceProvider,
                                    Func<IServiceProvider, PageParameters<TPage>, TViewModel> constructViewModel) {
            _serviceProvider = serviceProvider;
            _constructViewModel = constructViewModel;
        }

        public IPageViewModel<TPage> Create(TPage content, PageExtensionData extensionData) {
            var blockParameters = new PageParameters<TPage>(content, extensionData);
        
            return _constructViewModel(_serviceProvider, blockParameters);
        }

        public IPageViewModel Create(IPublishedContent content, PageExtensionData extensionData) {
            return Create((TPage) content, extensionData);
        }
    }

    public static class PageViewModelFactory {
        public static IPageViewModelFactory Default(IServiceProvider serviceProvider, Type blockType) {
            return (IPageViewModelFactory) typeof(PageViewModelFactory).CallStaticMethod(nameof(Default))
                                                                       .OfGenericType(blockType)
                                                                       .WithParameter(typeof(IServiceProvider), serviceProvider)
                                                                       .Run();
        }
    
        public static PageViewModelFactory<TPage, PageViewModel<TPage>> Default<TPage>(IServiceProvider serviceProvider)
            where TPage : class, IPublishedContent {
            return new PageViewModelFactory<TPage, PageViewModel<TPage>>(serviceProvider,
                                                                         (_, p) => new PageViewModel<TPage>(p));
        }
    }
}

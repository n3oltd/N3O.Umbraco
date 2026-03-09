using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages;

public class PageViewModelFactory<TPage, TViewModel> : IPageViewModelFactory<TPage>
    where TPage : IPublishedContent
    where TViewModel : IPageViewModel<TPage> {
    private readonly IServiceProvider _serviceProvider;
    private readonly Func<IServiceProvider, PageParameters<TPage>, TViewModel> _constructViewModel;

    public PageViewModelFactory(IServiceProvider serviceProvider,
                                Func<IServiceProvider, PageParameters<TPage>, TViewModel> constructViewModel) {
        _serviceProvider = serviceProvider;
        _constructViewModel = constructViewModel;
    }

    public IPageViewModel<TPage> Create(TPage content, PageModulesData modulesData) {
        var stringLocalizer = _serviceProvider.GetRequiredService<IStringLocalizer>();
        
        var blockParameters = new PageParameters<TPage>(s => stringLocalizer.Get(TextFolders.Pages,
                                                                                 content.ContentType.Alias.Pascalize(),
                                                                                 s),
                                                        content,
                                                        modulesData);
    
        return _constructViewModel(_serviceProvider, blockParameters);
    }

    public IPageViewModel Create(IPublishedContent content, PageModulesData modulesData) {
        return Create((TPage) content, modulesData);
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
        where TPage : IPublishedContent {
        return new PageViewModelFactory<TPage, PageViewModel<TPage>>(serviceProvider,
                                                                     (_, p) => new PageViewModel<TPage>(p));
    }
}

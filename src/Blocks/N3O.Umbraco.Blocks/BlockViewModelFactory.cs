using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public class BlockViewModelFactory<TBlock, TSettings, TViewModel> : IBlockViewModelFactory<TBlock, TSettings>
    where TBlock : class, IPublishedElement
    where TViewModel : IBlockViewModel<TBlock, TSettings> {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Func<IServiceProvider, BlockParameters<TBlock, TSettings>, TViewModel> _constructViewModel;

    public BlockViewModelFactory(IHttpContextAccessor httpContextAccessor,
                                 Func<IServiceProvider, BlockParameters<TBlock, TSettings>, TViewModel> constructViewModel) {
        _httpContextAccessor = httpContextAccessor;
        _constructViewModel = constructViewModel;
    }

    public IBlockViewModel Create(IPublishedElement content, object settings) {
        if (content is not TBlock typedContent || settings is not TSettings typedSettings) {
            return null;
        }
        
        return Create(typedContent, typedSettings, content.Key);
    }

    private IBlockViewModel<TBlock, TSettings> Create(TBlock content, TSettings settings, Guid id) {
        var serviceProvider = _httpContextAccessor.HttpContext.RequestServices;
        
        var stringLocalizer = serviceProvider.GetRequiredService<IStringLocalizer>();
        var blockPipeline = serviceProvider.GetRequiredService<IBlockPipeline>();
        var modulesData = blockPipeline.RunAsync(content).GetAwaiter().GetResult();
        
        var blockParameters = new BlockParameters<TBlock, TSettings>(s => stringLocalizer.Get(TextFolders.Blocks,
                                                                                              content.ContentType.Alias.Pascalize(),
                                                                                              s),
                                                                     content,
                                                                     settings,
                                                                     modulesData,
                                                                     id);
        
        return _constructViewModel(serviceProvider, blockParameters);
    }
}

public static class BlockViewModelFactory {
    public static IBlockViewModelFactory Default(IHttpContextAccessor httpContextAccessor, Type blockType) {
        return (IBlockViewModelFactory) typeof(BlockViewModelFactory).CallStaticMethod(nameof(Default))
                                                                     .OfGenericType(blockType)
                                                                     .WithParameter(typeof(IHttpContextAccessor), httpContextAccessor)
                                                                     .Run();
    }

    public static BlockViewModelFactory<TBlock, TSettings, BlockViewModel<TBlock, TSettings>> Default<TBlock, TSettings>(IHttpContextAccessor httpContextAccessor)
        where TBlock : class, IPublishedElement {
        return new BlockViewModelFactory<TBlock, TSettings, BlockViewModel<TBlock, TSettings>>(httpContextAccessor,
                                                                                               (_, p) => new BlockViewModel<TBlock, TSettings>(p));
    }
}

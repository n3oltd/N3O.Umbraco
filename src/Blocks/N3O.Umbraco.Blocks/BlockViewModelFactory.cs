using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public class BlockViewModelFactory<TBlock, TViewModel> : IBlockViewModelFactory<TBlock>
    where TBlock : class, IPublishedElement
    where TViewModel : IBlockViewModel<TBlock> {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Func<IServiceProvider, BlockParameters<TBlock>, TViewModel> _constructViewModel;

    public BlockViewModelFactory(IHttpContextAccessor httpContextAccessor,
                                 Func<IServiceProvider, BlockParameters<TBlock>, TViewModel> constructViewModel) {
        _httpContextAccessor = httpContextAccessor;
        _constructViewModel = constructViewModel;
    }

    public IBlockViewModel Create(IPublishedElement content, Guid id) {
        if (content is not TBlock typedContent) {
            return null;
        }
        
        return Create(typedContent, id);
    }

    private IBlockViewModel<TBlock> Create(TBlock content, Guid id) {
        var serviceProvider = _httpContextAccessor.HttpContext.RequestServices;
        
        var stringLocalizer = serviceProvider.GetRequiredService<IStringLocalizer>();
        var blockPipeline = serviceProvider.GetRequiredService<IBlockPipeline>();
        var modulesData = blockPipeline.RunAsync(content).GetAwaiter().GetResult();
        
        var blockParameters = new BlockParameters<TBlock>(s => stringLocalizer.Get(TextFolders.Blocks,
                                                                                   content.ContentType.Alias.Pascalize(),
                                                                                   s),
                                                          content,
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

    public static BlockViewModelFactory<TBlock, BlockViewModel<TBlock>> Default<TBlock>(IHttpContextAccessor httpContextAccessor)
        where TBlock : class, IPublishedElement {
        return new BlockViewModelFactory<TBlock, BlockViewModel<TBlock>>(httpContextAccessor,
                                                                         (_, p) => new BlockViewModel<TBlock>(p));
    }
}

using N3O.Umbraco.Extensions;
using Perplex.ContentBlocks.Rendering;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public class BlockViewModelFactory<TBlock, TViewModel> : ContentBlockViewModelFactory<TBlock>
    where TBlock : class, IPublishedElement
    where TViewModel : IBlockViewModel<TBlock> {
    private readonly IServiceProvider _serviceProvider;
    private readonly Func<IServiceProvider, BlockParameters<TBlock>, TViewModel> _constructViewModel;

    public BlockViewModelFactory(IServiceProvider serviceProvider,
                                 Func<IServiceProvider, BlockParameters<TBlock>, TViewModel> constructViewModel) {
        _serviceProvider = serviceProvider;
        _constructViewModel = constructViewModel;
    }

    public override IContentBlockViewModel<TBlock> Create(TBlock content, Guid id, Guid definitionId, Guid layoutId) {
        var blockParameters = new BlockParameters<TBlock>(content, id, definitionId, layoutId);
        
        return _constructViewModel(_serviceProvider, blockParameters);
    }
}

public static class BlockViewModelFactory {
    public static IContentBlockViewModelFactory Default(IServiceProvider serviceProvider, Type blockType) {
        return (IContentBlockViewModelFactory) typeof(BlockViewModelFactory).CallStaticMethod(nameof(Default))
                                                                            .OfGenericType(blockType)
                                                                            .WithParameter(typeof(IServiceProvider), serviceProvider)
                                                                            .Run();
    }
    
    public static BlockViewModelFactory<TBlock, BlockViewModel<TBlock>> Default<TBlock>(IServiceProvider serviceProvider)
        where TBlock : class, IPublishedElement {
        return new BlockViewModelFactory<TBlock, BlockViewModel<TBlock>>(serviceProvider,
                                                                         (_, p) => new BlockViewModel<TBlock>(p));
    }
}

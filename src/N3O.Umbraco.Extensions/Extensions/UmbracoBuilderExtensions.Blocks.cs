using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Blocks;
using Perplex.ContentBlocks.Rendering;
using System;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Extensions;

public static partial class UmbracoBuilderExtensions {
    public static IUmbracoBuilder AddBlockViewModel<TBlock, TViewModel>(this IUmbracoBuilder builder,
                                                                        Func<BlockParameters<TBlock>, TViewModel> constructor)
        where TViewModel : IBlockViewModel<TBlock>
        where TBlock : class, IPublishedElement {
        return AddBlockViewModel<TBlock, TViewModel, None>(builder, (p, _) => constructor(p));
    }

    public static IUmbracoBuilder AddBlockViewModel<TBlock, TViewModel, T>(this IUmbracoBuilder builder,
                                                                           Func<BlockParameters<TBlock>, T, TViewModel> constructor)
        where TViewModel : IBlockViewModel<TBlock>
        where TBlock : class, IPublishedElement {
        return AddBlockViewModel<TBlock, TViewModel, T, None>(builder, (p, arg, _) => constructor(p, arg));
    }

    public static IUmbracoBuilder AddBlockViewModel<TBlock, TViewModel, T1, T2>(this IUmbracoBuilder builder,
                                                                                Func<BlockParameters<TBlock>, T1, T2, TViewModel> constructor)
        where TViewModel : IBlockViewModel<TBlock>
        where TBlock : class, IPublishedElement {
        return AddBlockViewModel<TBlock, TViewModel, T1, T2, None>(builder,
                                                                   (p, arg1, arg2, _) => constructor(p, arg1, arg2));
    }

    public static IUmbracoBuilder AddBlockViewModel<TBlock, TViewModel, T1, T2, T3>(this IUmbracoBuilder builder,
                                                                                    Func<BlockParameters<TBlock>, T1, T2, T3, TViewModel> constructor)
        where TViewModel : IBlockViewModel<TBlock>
        where TBlock : class, IPublishedElement {
        return AddBlockViewModel<TBlock, TViewModel, T1, T2, T3, None>(builder,
                                                                       (p, arg1, arg2, arg3, _) => constructor(p, arg1, arg2, arg3));
    }
    
    public static IUmbracoBuilder AddBlockViewModel<TBlock, TViewModel, T1, T2, T3, T4>(this IUmbracoBuilder builder,
                                                                                        Func<BlockParameters<TBlock>, T1, T2, T3, T4, TViewModel> constructor)
        where TViewModel : IBlockViewModel<TBlock>
        where TBlock : class, IPublishedElement {
        return AddBlockViewModel<TBlock, TViewModel, T1, T2, T3, T4, None>(builder,
                                                                           (p, arg1, arg2, arg3, arg4, _) => constructor(p, arg1, arg2, arg3, arg4));
                                                                           
    }
    
    public static IUmbracoBuilder AddBlockViewModel<TBlock, TViewModel, T1, T2, T3, T4, T5>(this IUmbracoBuilder builder,
                                                                                            Func<BlockParameters<TBlock>, T1, T2, T3, T4, T5, TViewModel> constructor)
        where TViewModel : IBlockViewModel<TBlock>
        where TBlock : class, IPublishedElement {
        AddBlockViewModel<TBlock, TViewModel>(builder, (s, p) => {
            var arg1 = s.GetRequiredService<T1>();
            var arg2 = s.GetRequiredService<T2>();
            var arg3 = s.GetRequiredService<T3>();
            var arg4 = s.GetRequiredService<T4>();
            var arg5 = s.GetRequiredService<T5>();
            
            return constructor(p, arg1, arg2, arg3, arg4, arg5);
        });

        return builder;
    }

    public static IUmbracoBuilder AddDefaultBlockViewModel(this IUmbracoBuilder builder, Type blockType) {
        builder.Services.AddTransient(typeof(IContentBlockViewModelFactory<>).MakeGenericType(blockType),
                                      s => BlockViewModelFactory.Default(s, blockType));

        return builder;
    }
    
    private static void AddBlockViewModel<TBlock, TViewModel>(IUmbracoBuilder builder,
                                                              Func<IServiceProvider, BlockParameters<TBlock>, TViewModel> constructor)
        where TViewModel : IBlockViewModel<TBlock>
        where TBlock : class, IPublishedElement {
        builder.Services.AddTransient<IContentBlockViewModelFactory<TBlock>>(s => new BlockViewModelFactory<TBlock, TViewModel>(s, constructor));
    }
}
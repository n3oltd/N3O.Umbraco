/*
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks.Extensions;

public static class UmbracoBuilderExtensions {
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
    
    public static IUmbracoBuilder AddBlockViewModel<TBlock, TViewModel, T1, T2, T3, T4, T5, T6>(this IUmbracoBuilder builder,
                                                                                                Func<BlockParameters<TBlock>, T1, T2, T3, T4, T5, T6, TViewModel> constructor)
        where TViewModel : IBlockViewModel<TBlock>
        where TBlock : class, IPublishedElement {
        AddBlockViewModel<TBlock, TViewModel>(builder, (s, p) => {
            var arg1 = s.GetRequiredService<T1>();
            var arg2 = s.GetRequiredService<T2>();
            var arg3 = s.GetRequiredService<T3>();
            var arg4 = s.GetRequiredService<T4>();
            var arg5 = s.GetRequiredService<T5>();
            var arg6 = s.GetRequiredService<T6>();
        
            return constructor(p, arg1, arg2, arg3, arg4, arg5, arg6);
        });

        return builder;
    }

    public static IUmbracoBuilder AddDefaultBlockViewModel(this IUmbracoBuilder builder, Type blockType) {
        builder.Services.TryAddTransient(typeof(IBlockViewModelFactory<>).MakeGenericType(blockType),
                                         s => {
                                             var httpContextAccessor = s.GetRequiredService<IHttpContextAccessor>();
                                             
                                             return BlockViewModelFactory.Default(httpContextAccessor, blockType);
                                         });

        return builder;
    }

    private static void AddBlockViewModel<TBlock, TViewModel>(IUmbracoBuilder builder,
                                                                     Func<IServiceProvider, BlockParameters<TBlock>, TViewModel> constructor)
        where TViewModel : IBlockViewModel<TBlock>
        where TBlock : class, IPublishedElement {
        builder.Services.AddTransient<IBlockViewModelFactory<TBlock>>(s => {
            var httpContextAccessor = s.GetRequiredService<IHttpContextAccessor>();
            
            return new BlockViewModelFactory<TBlock, TViewModel>(httpContextAccessor, constructor);
        });
    }
}
*/

using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.DependencyInjection;
using System;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Blazor.Extensions;

public static class UmbracoBuilderExtensions {
    public static IUmbracoBuilder AddBlazor(this IUmbracoBuilder builder, Action<CircuitOptions> configureOptions) {
        builder.Services.AddServerSideBlazor(configureOptions);

        return builder;
    }
}
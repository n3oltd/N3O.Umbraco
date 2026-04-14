using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Localization;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Extensions;

public static partial class UmbracoBuilderExtensions {
    public static void UsesStagingSite(this IUmbracoBuilder builder) {
        if (Composer.WebHostEnvironment.IsProduction()) {
            builder.Services.AddSingleton<IStringLocalizer, ReadOnlyStringLocalizer>();
        }
    }
} 
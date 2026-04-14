using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Localization;

public static class StringLocalizer {
    public static IStringLocalizer Instance { get; } = StaticServiceProvider.Instance.GetRequiredService<IStringLocalizer>();
}

using Microsoft.Extensions.DependencyInjection.Extensions;
using N3O.Umbraco.Composing;
using NodaTime;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Localization;

public class LocalizationComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.TryAddSingleton<IClock>(SystemClock.Instance);
        builder.Services.TryAddSingleton<IDateTimeFormatter, DateTimeFormatter>();
        builder.Services.TryAddSingleton<IFormatter, Formatter>();
        builder.Services.TryAddSingleton<IFormatterFactory, FormatterFactory>();
        builder.Services.TryAddSingleton<ILocalClock, LocalClock>();
        builder.Services.TryAddSingleton<ILocalizationSettingsAccessor, EnvironmentLocalizationSettingsAccessor>();
        builder.Services.TryAddSingleton<INumberFormatter, NumberFormatter>();
        builder.Services.TryAddSingleton<IStringLocalizer, ReadWriteStringLocalizer>();
        builder.Services.TryAddSingleton<ITextFormatter, TextFormatter>();
    }
}

using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using NodaTime;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Localization;

public class LocalizationComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IClock>(SystemClock.Instance);
        builder.Services.AddSingleton<IDateTimeFormatter, DateTimeFormatter>();
        builder.Services.AddSingleton<IFormatter, Formatter>();
        builder.Services.AddSingleton<ILocalClock, LocalClock>();
        builder.Services.AddSingleton<ILocalizationSettingsAccessor, LocalizationSettingsAccessor>();
        builder.Services.AddSingleton<INumberFormatter, NumberFormatter>();
        builder.Services.AddSingleton<IStringLocalizer, StringLocalizer>();
        builder.Services.AddSingleton<ITextFormatter, TextFormatter>();
    }
}

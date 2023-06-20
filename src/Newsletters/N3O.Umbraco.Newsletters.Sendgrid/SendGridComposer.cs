using SendGrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Newsletters.SendGrid.Content;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Newsletters.SendGrid;

public class SendGridComposer : Composer
{
    public override void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddTransient<INewslettersClient>(serviceProvider =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<SendGridNewsletterClient>>();
            var textFormatter = serviceProvider.GetRequiredService<ITextFormatter>();
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var settings = contentCache.Single<SendGridSettingsContent>();
            var baseClient = new SendGridClient(settings.ApiKey);
            var client = new SendGridNewsletterClient(logger, textFormatter, baseClient, settings.AudienceId);
            return client;
        });
    }
}

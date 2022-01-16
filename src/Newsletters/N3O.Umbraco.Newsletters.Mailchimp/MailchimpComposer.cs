using MailChimp.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Newsletters.Mailchimp.Content;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Newsletters.Mailchimp {
    public class MailchimpComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<INewslettersClient>(serviceProvider => {
                var logger = serviceProvider.GetRequiredService<ILogger<MailchimpClient>>();
                var textFormatter = serviceProvider.GetRequiredService<ITextFormatter>();
                var contentCache = serviceProvider.GetRequiredService<IContentCache>();
                var settings = contentCache.Single<MailchimpSettingsContent>();
        
                var manager = new MailChimpManager(settings.ApiKey);
                var client = new MailchimpClient(logger, textFormatter, manager, settings.AudienceId);

                return client;
            });
        }
    }
}

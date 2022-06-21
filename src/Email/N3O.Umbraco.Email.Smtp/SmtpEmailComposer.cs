using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Email.Smtp.Content;
using System.Net;
using System.Net.Mail;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Email.Smtp;

public class SmtpEmailComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<ISender>(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var settings = contentCache.Single<SmtpSettingsContent>();

            var smtpClient = new SmtpClient(settings.Host, settings.Port);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(settings.Username, settings.Password);

            return new SmtpSender(smtpClient);
        });
    }
}

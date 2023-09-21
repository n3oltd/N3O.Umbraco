using FluentEmail.Core.Interfaces;
using FluentEmail.Smtp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Email.Smtp.Content;
using System.Net;
using System.Net.Mail;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Email.Smtp;

public class SmtpEmailComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<ISender>(serviceProvider => {
            var contentCache = serviceProvider.GetRequiredService<IContentCache>();
            var contentSettings = contentCache.Single<SmtpSettingsContent>();

            string host;
            int port;
            string username;
            string password;

            if (contentSettings != null) {
                host = contentSettings.Host;
                port = contentSettings.Port;
                username = contentSettings.Username;
                password = contentSettings.Password;
            } else {
                var appSettings = serviceProvider.GetRequiredService<IOptions<GlobalSettings>>().Value.Smtp;
                
                host = appSettings.Host;
                port = appSettings.Port;
                username = appSettings.Username;
                password = appSettings.Password;
            }
            
            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(username, password);

            return new SmtpSender(smtpClient);
        });
    }
}

using MimeKit;
using N3O.Umbraco.Email.Extensions;
using N3O.Umbraco.Email.Lookups;
using N3O.Umbraco.Email.Models;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Email;

public class MimeMessageBuilder : IMimeMessageBuilder {
    public MimeMessage BuildMessage(EmailIdentity from,
                                    IEnumerable<EmailIdentity> to,
                                    IEnumerable<EmailIdentity> cc,
                                    IEnumerable<EmailIdentity> bcc,
                                    string subject,
                                    string body,
                                    BodyFormat bodyFormat,
                                    IEnumerable<EmailAttachment> attachments) {
        var message = new MimeMessage();

        message.From.Add(from.ToMailboxAddress());

        to.OrEmpty().Do(x => message.To.Add(x.ToMailboxAddress()));

        cc.OrEmpty().Do(x => message.Cc.Add(x.ToMailboxAddress()));

        bcc.OrEmpty().Do(x => message.Bcc.Add(x.ToMailboxAddress()));

        message.Subject = subject;

        var bodyBuilder = new BodyBuilder {
            HtmlBody = bodyFormat == BodyFormats.Html ? body : null,
            TextBody = bodyFormat == BodyFormats.Text ? body : null
        };

        foreach (var attachment in attachments.OrEmpty()) {
            var contentType = attachment.ContentType.ToContentType();

            bodyBuilder.Attachments.Add(attachment.Name, attachment.Bytes, contentType);
        }

        message.Body = bodyBuilder.ToMessageBody();

        return message;
    }
}
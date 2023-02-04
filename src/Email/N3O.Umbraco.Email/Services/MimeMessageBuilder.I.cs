using MimeKit;
using N3O.Umbraco.Email.Lookups;
using N3O.Umbraco.Email.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Email;

public interface IMimeMessageBuilder {
    MimeMessage BuildMessage(EmailIdentity from,
                             IEnumerable<EmailIdentity> to,
                             IEnumerable<EmailIdentity> cc,
                             IEnumerable<EmailIdentity> bcc,
                             string subject,
                             string body,
                             BodyFormat bodyFormat,
                             IEnumerable<EmailAttachment> attachments);
}

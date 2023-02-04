using MimeKit;
using N3O.Umbraco.Email.Models;

namespace N3O.Umbraco.Email.Extensions;

public static class EmailIdentityExtensions {
    public static MailboxAddress ToMailboxAddress(this EmailIdentity emailIdentity) {
        return new MailboxAddress(emailIdentity.Name, emailIdentity.Email);
    }
}
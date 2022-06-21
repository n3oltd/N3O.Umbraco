using MailChimp.Net.Core;
using System;
using System.Net;

namespace N3O.Umbraco.Newsletters.Mailchimp.Extensions;

public static class MailChimpExceptionExtensions {
    public static bool IsNotFound(this Exception exception) {
        return exception is MailChimpNotFoundException;
    }

    public static bool IsInvalidApiKey(this Exception exception) {
        if (exception is MailChimpException mailChimpException) {
            if (mailChimpException.Status == (int) HttpStatusCode.Unauthorized) {
                return true;
            }
        }

        return false;
    }
}

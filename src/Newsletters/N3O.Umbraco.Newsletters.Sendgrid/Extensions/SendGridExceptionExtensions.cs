using System;
using SendGrid.Helpers.Errors.Model;

namespace N3O.Umbraco.Newsletters.SendGrid.Extensions;

public static class SendGridExceptionExtensions {
    public static bool IsNotFound(this Exception exception) {
        return exception is NotFoundException;
    }

    public static bool IsInvalidApiKey(this Exception exception) {
        return exception is UnauthorizedException;
    }
}
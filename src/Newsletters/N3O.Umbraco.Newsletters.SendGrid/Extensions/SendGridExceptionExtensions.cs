using SendGrid.Helpers.Errors.Model;
using System;

namespace N3O.Umbraco.Newsletters.SendGrid.Extensions;

public static class SendGridExceptionExtensions {
    public static bool IsInvalidApiKey(this Exception exception) {
        if (exception is UnauthorizedException) {
            return true;
        }

        return false;
    }
    
    public static bool IsNotFound(this Exception exception) {
        return exception is NotFoundException;
    }
}
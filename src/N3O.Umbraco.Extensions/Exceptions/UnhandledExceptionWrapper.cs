using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Exceptions;

public class UnhandledExceptionWrapper : ExceptionWithProblemDetails {
    public UnhandledExceptionWrapper(Exception exception) {
        Exception = exception;
    }

    public Exception Exception { get; }

    public override ProblemDetails GetProblemDetails(IFormatter formatter) {
        var title = formatter.Text.Format<Strings>(s => s.GeneralError);
        var detail = Exception.Message;

        var problemDetails = new ProblemDetails(System.Net.HttpStatusCode.InternalServerError, title, detail);

        return problemDetails;
    }

    public class Strings : CodeStrings {
        public string GeneralError => "Sorry, an error has occurred";
    }
}
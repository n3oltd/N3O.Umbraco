using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Cloud.Exceptions;

public class CloudApiException : ExceptionWithProblemDetails {
    private readonly ProblemDetails _problemDetails;

    public CloudApiException(ProblemDetails problemDetails, Exception exception) {
        _problemDetails = problemDetails;
        
        Exception = exception;
    }
    
    public Exception Exception { get; }

    public override ProblemDetails GetProblemDetails(IFormatter _) {
        return _problemDetails;
    }
}
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Crm.Engage.Exceptions;

public class ServiceClientException : ExceptionWithProblemDetails {
    private readonly ProblemDetails _problemDetails;

    public ServiceClientException(Exception ex, ProblemDetails problemDetails) {
        _problemDetails = problemDetails;
    }

    public override ProblemDetails GetProblemDetails(IFormatter formatter) {
        return _problemDetails;
    }
}
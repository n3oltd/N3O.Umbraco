using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Validation;
using System;

namespace N3O.Umbraco.Crm.Engage.Exceptions;

public class ServiceClientException : Exception {
    public ServiceClientException(ProblemDetails problemDetails) : base(problemDetails.ToString()) { }

    public ServiceClientException(Exception ex, ValidationProblemDetails validationProblemDetails)
        : base(validationProblemDetails.ToString(), ex) { }
}
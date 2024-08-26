using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Engage.Exceptions;

public class ServiceClientException : Exception {
    public ServiceClientException(ProblemDetails problemDetails) : base(problemDetails.ToString()) { }

    public ServiceClientException(Exception ex, ValidationProblemDetails validationProblemDetails)
        : base(validationProblemDetails.ToString(), ex) { }
}
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Extensions;

namespace N3O.Umbraco.Validation;

public class ValidationException : ExceptionWithProblemDetails {
    public ValidationException(ValidationFailure failure) : this(failure.Yield()) { }

    public ValidationException(IEnumerable<ValidationFailure> failures) {
        Failures = failures.ToList();
    }

    public IReadOnlyList<ValidationFailure> Failures { get; }

    public override string Message {
        get {
            var sb = new StringBuilder();

            foreach (var (failure, i) in Failures.Select((failure, i) => (failure, i))) {
                sb.AppendLine($"Failure {i}: {failure.Error}");
            }

            return sb.ToString();
        }
    }

    public override ProblemDetails GetProblemDetails(IFormatter formatter) {
        var problemDetails = new ValidationProblemDetails(formatter, Failures);

        return problemDetails;
    }
}
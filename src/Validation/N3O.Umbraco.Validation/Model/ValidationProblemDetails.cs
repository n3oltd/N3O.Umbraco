using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Localization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace N3O.Umbraco.Validation;

public class ValidationProblemDetails : ProblemDetails {
    [JsonConstructor]
    public ValidationProblemDetails(HttpStatusCode status,
                                    string title,
                                    string detail,
                                    IEnumerable<ValidationFailure> errors) 
        : base(status, title, detail) {
        Errors = errors;
    }
    
    public ValidationProblemDetails(IFormatter formatter, IEnumerable<ValidationFailure> failures)
        : base(HttpStatusCode.PreconditionFailed,
               formatter.Text.Format<Strings>(s => s.ValidationFailed),
               formatter.Text.Format<Strings>(s => s.SeeErrors)) {
        ValidationFailure GetFailureDetails(ValidationFailure f) {
            var failureDetails = new ValidationFailure(f.Property, f.Error);

            return failureDetails;
        }

        Errors = failures.Select(GetFailureDetails).ToList();
    }

    public IEnumerable<ValidationFailure> Errors { get; }

    public class Strings : CodeStrings {
        public string ValidationFailed => "Validation failed";
        public string SeeErrors => $"See {nameof(Errors).ToLowerInvariant()} property for details";
    }
}
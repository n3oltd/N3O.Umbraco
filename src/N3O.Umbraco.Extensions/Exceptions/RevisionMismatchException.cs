using N3O.Umbraco.Entities;
using N3O.Umbraco.Localization;
using System.Net;

namespace N3O.Umbraco.Exceptions {
    public class RevisionMismatchException : ExceptionWithProblemDetails {
        public RevisionMismatchException(RevisionId revisionId) {
            RevisionId = revisionId;
        }

        public RevisionId RevisionId { get; }

        public override ProblemDetails GetProblemDetails(IFormatter formatter) {
            var problemDetails = new ProblemDetails(HttpStatusCode.Conflict,
                                                    "Error",
                                                    "This record has been updated since you last loaded the page and cannot be saved. Please refresh and try again.");

            return problemDetails;
        }
    }
}
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Exceptions {
    public abstract class ExceptionWithProblemDetails : Exception {
        public abstract ProblemDetails GetProblemDetails(IFormatter formatter);
    }
}
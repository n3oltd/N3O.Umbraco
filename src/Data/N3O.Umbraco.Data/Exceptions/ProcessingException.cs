using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Exceptions {
    public class ProcessingException : Exception {
        public ProcessingException(IEnumerable<string> errors) {
            Errors = errors.OrEmpty().ToList();
        }

        public IReadOnlyList<string> Errors { get; }
    }
}
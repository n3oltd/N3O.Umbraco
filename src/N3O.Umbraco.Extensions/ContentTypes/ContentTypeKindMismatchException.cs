using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.ContentTypes;

// Whether a type is an element or a document decides where its content can live, so a site that holds one as
// the other cannot be converged by reassigning the flag: doing so would convert the type under whatever
// already uses it. Callers that build schema use this to report the disagreement instead
public class ContentTypeKindMismatchException : Exception {
    public ContentTypeKindMismatchException(string alias, bool isElement)
        : base($"Content type {alias.Quote()} is held by the site as " +
               $"{(isElement ? "an element" : "a document")} type, so it cannot be converged as " +
               $"{(isElement ? "a document" : "an element")} type") {
        Alias = alias;
        IsElement = isElement;
    }

    public string Alias { get; }
    public bool IsElement { get; }
}

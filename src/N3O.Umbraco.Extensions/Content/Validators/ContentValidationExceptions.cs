using System;
using Umbraco.Cms.Core.Events;

namespace N3O.Umbraco.Content;

public class ContentValidationErrorException : ContentValidationException {
    public ContentValidationErrorException(string message) {
        EventMessage = new EventMessage("Error", message, EventMessageType.Error);
    }

    public override EventMessage EventMessage { get; }
}

public class ContentValidationWarningException : ContentValidationException {
    public ContentValidationWarningException(string message) {
        EventMessage = new EventMessage("Warning", message, EventMessageType.Warning);
    }

    public override EventMessage EventMessage { get; }
}

public abstract class ContentValidationException : Exception {
    public abstract EventMessage EventMessage { get; }
}

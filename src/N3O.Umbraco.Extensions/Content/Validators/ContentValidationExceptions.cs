using System;
using Umbraco.Cms.Core.Events;

namespace N3O.Umbraco.Content;

public class ContentValidationErrorException : ContentValidationException {
    public ContentValidationErrorException(string message) {
        PopupMessage = new EventMessage("Error", message, EventMessageType.Error);
    }
    
    public override EventMessage PopupMessage { get; }
}

public class ContentValidationWarningException : ContentValidationException {
    public ContentValidationWarningException(string message) {
        PopupMessage = new EventMessage("Warning", message, EventMessageType.Warning);
    }

    public override EventMessage PopupMessage { get; }
}

public abstract class ContentValidationException : Exception {
    public abstract EventMessage PopupMessage { get; }
}

using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Content;

public abstract class ContentValidator : IContentValidator {
    protected ContentValidator(IContentHelper contentHelper) {
        ContentHelper = contentHelper;
    }

    protected void ErrorResult(IContentProperty contentProperty, string message) {
        ErrorResult($"Property {contentProperty.Name.Quote()} {message}");
    }

    protected void ErrorResult(string message) {
        throw new ContentValidationErrorException(message);
    }

    protected void WarningResult(IContentProperty contentProperty, string message) {
        WarningResult($"Property {contentProperty.Name.Quote()} {message}");
    }

    protected void WarningResult(string message) {
        throw new ContentValidationWarningException(message);
    }

    public abstract bool IsValidator(ContentProperties content);
    public abstract void Validate(ContentProperties content);

    protected IContentHelper ContentHelper { get; }
}

using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Content {
    public abstract class ContentValidator : IContentValidator {
        protected ContentValidator(IContentHelper contentHelper) {
            ContentHelper = contentHelper;
        }

        protected void ErrorResult(ContentProperty contentProperty, string message) {
            ErrorResult($"Property {contentProperty.Name.Quote()} {message}");
        }

        protected void ErrorResult(string message) {
            throw new ContentValidationErrorException(message);
        }

        protected void WarningResult(ContentProperty contentProperty, string message) {
            WarningResult($"Property {contentProperty.Name.Quote()} {message}");
        }

        protected void WarningResult(string message) {
            throw new ContentValidationWarningException(message);
        }

        public abstract bool IsValidator(ContentNode content);
        public abstract void Validate(ContentNode content);
    
        protected IContentHelper ContentHelper { get; }
    }
}

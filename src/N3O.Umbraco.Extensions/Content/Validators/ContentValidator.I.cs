namespace N3O.Umbraco.Content {
    public interface IContentValidator {
        bool IsValidator(ContentNode content);
        void Validate(ContentNode content);
    }
}

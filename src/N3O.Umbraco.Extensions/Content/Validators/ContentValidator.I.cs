namespace N3O.Umbraco.Content {
    public interface IContentValidator {
        bool IsValidator(ContentProperties content);
        void Validate(ContentProperties content);
    }
}

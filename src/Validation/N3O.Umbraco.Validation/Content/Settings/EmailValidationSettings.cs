using N3O.Umbraco.Content;

namespace N3O.Umbraco.Validation.Content {
    public class EmailValidationSettings : UmbracoContent<EmailValidationSettings> {
        public bool ValidateEmails => GetValue(x => x.ValidateEmails);
    }
}

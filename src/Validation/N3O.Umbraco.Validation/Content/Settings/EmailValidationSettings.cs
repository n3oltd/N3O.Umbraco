using N3O.Umbraco.Content;

namespace N3O.Umbraco.Validation.Content {
    public class EmailValidationSettings : UmbracoContent {
        public bool ValidateEmails => GetValue<EmailValidationSettings, bool>(x => x.ValidateEmails);
    }
}

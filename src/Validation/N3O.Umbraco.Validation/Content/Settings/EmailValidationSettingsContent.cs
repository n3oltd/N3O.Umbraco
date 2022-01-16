using N3O.Umbraco.Content;

namespace N3O.Umbraco.Validation.Content {
    public class EmailValidationSettingsContent : UmbracoContent<EmailValidationSettingsContent> {
        public bool ValidateEmails => GetValue(x => x.ValidateEmails);
    }
}

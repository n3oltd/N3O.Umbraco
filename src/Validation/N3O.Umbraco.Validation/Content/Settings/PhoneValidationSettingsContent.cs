using N3O.Umbraco.Content;

namespace N3O.Umbraco.Validation.Content {
    public class PhoneValidationSettingsContent : UmbracoContent<PhoneValidationSettingsContent> {
        public bool RequireNumbers => GetValue(x => x.RequireNumbers);
        public bool ValidateNumbers => GetValue(x => x.ValidateNumbers);
    }
}

using N3O.Umbraco.Content;

namespace N3O.Umbraco.Validation.Content {
    public class PhoneValidationSettings : UmbracoContent {
        public bool RequireNumbers => GetValue<PhoneValidationSettings, bool>(x => x.RequireNumbers);
        public bool ValidateNumbers => GetValue<PhoneValidationSettings, bool>(x => x.ValidateNumbers);
    }
}

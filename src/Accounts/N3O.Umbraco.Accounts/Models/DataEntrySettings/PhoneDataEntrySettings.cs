using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class PhoneDataEntrySettings : FieldSettings {
        public PhoneDataEntrySettings(bool visible,
                                      bool required,
                                      string label,
                                      string helpText, 
                                      string path,
                                      int order,
                                      bool validate,
                                      Country defaultCountry)
            : base(visible, required, label, helpText, path, order, validate) {
            DefaultCountry = defaultCountry;
        }

        public Country DefaultCountry { get; }
        public override string Type => "Phone";
    }
}
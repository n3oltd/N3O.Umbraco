using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class PhoneFieldSettings : FieldSettings {
        public PhoneFieldSettings(bool visible,
                                  bool required,
                                  string label,
                                  string helpText,
                                  string path,
                                  int order = -1,
                                  bool validate = false,
                                  Capitalisation capitalisation = null) :
            base(visible, required, label, helpText, path, order, validate) {
            Capitalisation = capitalisation;
        }

        public Capitalisation Capitalisation { get; }

        public override string Type => "Phone";
    }
}
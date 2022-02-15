namespace N3O.Umbraco.Accounts.Models {
    public class PhoneFieldSettings : FieldSettings {
        public PhoneFieldSettings(bool visible,
                                  bool required,
                                  string label,
                                  string helpText,
                                  string path,
                                  int order = -1,
                                  bool validate = false) :
            base(visible, required, label, helpText, path, order, validate) { }

        public override string Type => "Phone";
    }
}
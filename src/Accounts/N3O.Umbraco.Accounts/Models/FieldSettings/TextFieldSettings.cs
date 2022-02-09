using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class TextFieldSettings : FieldSettings {
        public TextFieldSettings(bool visible,
                                 bool required,
                                 string label,
                                 string helpText,
                                 int order = -1,
                                 bool validate = false,
                                 Capitalisation capitalisation = null) : 
            base(visible, required, label, helpText, order, validate) {
            Capitalisation = capitalisation;
        }

        public Capitalisation Capitalisation { get; }
        
        public override string Type => "Text";
    }
}
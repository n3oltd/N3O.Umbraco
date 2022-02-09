using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class TextFieldSettings : FieldSettings {
        public TextFieldSettings(bool required, string label, string helpText, int order, Capitalisation capitalisation, bool visible) : 
            base(required, label, helpText, order, visible) {
            Capitalisation = capitalisation;
        }

        public Capitalisation Capitalisation { get; }
    }
}
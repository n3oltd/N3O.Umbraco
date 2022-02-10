namespace N3O.Umbraco.Accounts.Models {
    public class EmailDataEntrySettings : TextFieldSettings {
        public EmailDataEntrySettings(bool visible,
                                      bool required,
                                      string label,
                                      string helpText,
                                      int order,
                                      bool validate)
            : base(visible, required, label, helpText, order, validate) {
        }

        public override string Type => "Email";
    }
}
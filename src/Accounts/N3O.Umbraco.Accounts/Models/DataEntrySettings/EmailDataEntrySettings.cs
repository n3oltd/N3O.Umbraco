namespace N3O.Umbraco.Accounts.Models;

public class EmailDataEntrySettings : TextFieldSettings {
    public EmailDataEntrySettings(bool visible,
                                  bool required,
                                  string label,
                                  string helpText,
                                  string path,
                                  int order,
                                  bool validate)
        : base(visible, required, label, helpText, path, order, validate) {
    }

    public override string Type => "Email";
}

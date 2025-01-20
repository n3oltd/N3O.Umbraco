using N3O.Umbraco.Accounts.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models;

public class NameDataEntrySettings : Value, IFieldSettingsCollection {
    public NameDataEntrySettings(SelectFieldSettings title,
                                 TextFieldSettings firstName,
                                 TextFieldSettings lastName,
                                 NameLayout layout) {
        Title = title;
        FirstName = firstName;
        LastName = lastName;
        Layout = layout;
    }

    public SelectFieldSettings Title { get; }
    public TextFieldSettings FirstName { get; }
    public TextFieldSettings LastName { get; }
    public NameLayout Layout { get; }

    public IEnumerable<FieldSettings> GetFieldSettings() {
        yield return Title;
        yield return FirstName;
        yield return LastName;
    }
}

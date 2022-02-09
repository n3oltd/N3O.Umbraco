using System.Collections.Generic;

namespace N3O.Umbraco.Accounts.Models {
    public class NameDataEntrySettings : Value, IFieldSettingsCollection {
        public NameDataEntrySettings(SelectFieldSettings title, TextFieldSettings firstName, TextFieldSettings lastName) {
            Title = title;
            FirstName = firstName;
            LastName = lastName;
        }

        public SelectFieldSettings Title { get; }
        public TextFieldSettings FirstName { get; }
        public TextFieldSettings LastName { get; }
        
        public IEnumerable<FieldSettings> GetFieldSettings() {
            yield return Title;
            yield return FirstName;
            yield return LastName;
        }
    }
}
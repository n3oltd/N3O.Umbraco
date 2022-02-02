namespace N3O.Umbraco.Accounts.Models {
    public class NameDataEntrySettings : Value {
        public NameDataEntrySettings(TitleDataEntrySettings title, FirstNameDataEntrySettings firstName, LastNameDataEntrySettings lastName) {
            Title = title;
            FirstName = firstName;
            LastName = lastName;
        }

        public TitleDataEntrySettings Title { get; }
        public FirstNameDataEntrySettings FirstName { get; }
        public LastNameDataEntrySettings LastName { get; }
    }
}
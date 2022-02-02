using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models {
    public class Name : Value, IName {
        [JsonConstructor]
        public Name(string title, string firstName, string lastName) {
            Title = title;
            FirstName = firstName;
            LastName = lastName;
        }

        public Name(IName name) : this(name.Title, name.FirstName, name.LastName) { }

        public string Title { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}
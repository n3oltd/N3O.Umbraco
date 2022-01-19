using N3O.Umbraco.Accounts.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public class NameRes : IName {
        public Title Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

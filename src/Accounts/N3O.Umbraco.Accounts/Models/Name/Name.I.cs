using N3O.Umbraco.Accounts.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public interface IName {
        Title Title { get; }
        string FirstName { get; }
        string LastName { get; }
    }
}

using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public interface IAddress {
        string Line1 { get; }
        string Line2 { get; }
        string Line3 { get; }
        string Locality { get; }
        string AdministrativeArea { get; }
        string PostalCode { get; }
        Country Country { get; }
    }
}

using N3O.Umbraco.TaxRelief.Lookups;

namespace N3O.Umbraco.Accounts.Models {
    public interface IAccount {
        IName Name { get; }
        IAddress Address { get; }
        ITelephone Telephone { get; }
        IEmail Email { get; }
        TaxStatus TaxStatus { get; }
    }
}

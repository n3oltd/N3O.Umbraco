using N3O.Umbraco.Accounts.Models;

namespace N3O.Umbraco.Payments.Models {
    public interface IBillingInfo {
        IAddress Address { get; }
        IEmail Email { get; }
        IName Name { get; }
        ITelephone Telephone { get; }
    }
}
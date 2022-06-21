using N3O.Umbraco.Accounts.Models;
using Stripe;

namespace N3O.Umbraco.Payments.Stripe;

public static class AddressExtensions {
    public static AddressOptions ToAddressOptions(this IAddress address) {
        var addressOptions = new AddressOptions();

        addressOptions.Line1 = address.Line1;
        addressOptions.Line2 = address.Line2;
        addressOptions.City = address.Locality;
        addressOptions.State = address.AdministrativeArea;
        addressOptions.PostalCode = address.PostalCode;
        addressOptions.Country = address.Country.Iso2Code;

        return addressOptions;
    }
}

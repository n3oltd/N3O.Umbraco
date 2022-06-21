using N3O.Umbraco.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class Address : Value, IAddress {
    [JsonConstructor]
    public Address(string line1,
                   string line2,
                   string line3,
                   string locality,
                   string administrativeArea,
                   string postalCode,
                   Country country) {
        Line1 = line1;
        Line2 = line2;
        Line3 = line3;
        Locality = locality;
        AdministrativeArea = administrativeArea;
        PostalCode = postalCode;
        Country = country;
    }

    public Address(IAddress address)
        : this(address.Line1,
               address.Line2,
               address.Line3,
               address.Locality,
               address.AdministrativeArea,
               address.PostalCode,
               address.Country) { }

    public string Line1 { get; }
    public string Line2 { get; }
    public string Line3 { get; }
    public string Locality { get; }
    public string AdministrativeArea { get; }
    public string PostalCode { get; }
    public Country Country { get; }
}

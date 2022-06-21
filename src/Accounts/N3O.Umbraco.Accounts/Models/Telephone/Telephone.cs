using N3O.Umbraco.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Accounts.Models;

public class Telephone : Value, ITelephone {
    [JsonConstructor]
    public Telephone(Country country, string number) {
        Country = country;
        Number = number;
    }

    public Telephone(ITelephone telephone) : this(telephone.Country, telephone.Number) { }

    public Country Country { get; }
    public string Number { get; }
}

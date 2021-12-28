using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Models;

public interface ITelephone {
    Country Country { get; }
    string Number { get; }
}

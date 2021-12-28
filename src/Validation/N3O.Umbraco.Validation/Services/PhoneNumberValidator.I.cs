using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Validation;

public interface IPhoneNumberValidator {
    bool IsValid(string number, Country country);
}

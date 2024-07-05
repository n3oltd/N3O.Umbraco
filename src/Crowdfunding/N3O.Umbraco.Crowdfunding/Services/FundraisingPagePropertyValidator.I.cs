using N3O.Umbraco.Crowdfunding.Lookups;

namespace N3O.Umbraco.CrowdFunding.Services;

public interface IFundraisingPagePropertyValidator {
    bool IsValidator(string alias);
    bool IsValid(string alias, PropertyType type);
}
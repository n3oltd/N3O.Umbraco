using N3O.Umbraco.Crowdfunding.Lookups;

namespace N3O.Umbraco.CrowdFunding;

public interface IContentPropertyValidator {
    bool IsValidator(string alias);
    bool IsValid(string alias, PropertyType type);
}
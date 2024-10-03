using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class CrowdfunderType : KeyedNamedLookup {
    public CrowdfunderType(string id, string name, uint key) : base(id, name, key) { }
}

public class CrowdfunderTypes : StaticLookupsCollection<CrowdfunderType> {
    public static readonly CrowdfunderType Campaign = new("campaign", "Campaign", 1);
    public static readonly CrowdfunderType Fundraiser = new("fundraiser", "Fundraiser", 2);

    public static CrowdfunderType GetByContentTypeAlias(string contentTypeAlias) {
        if (contentTypeAlias.EqualsInvariant(CrowdfundingConstants.Campaign.Alias)) {
            return Campaign;
        } else if (contentTypeAlias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias)) {
            return Fundraiser;
        } else {
            throw UnrecognisedValueException.For(contentTypeAlias);
        }
    }
}
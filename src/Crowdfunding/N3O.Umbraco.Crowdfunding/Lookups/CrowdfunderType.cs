using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class CrowdfunderType : NamedLookup {
    public CrowdfunderType(string id, string name) : base(id, name) { }
}

public class CrowdfunderTypes : StaticLookupsCollection<CrowdfunderType> {
    public static readonly CrowdfunderType Campaign = new("campaign", "Campaign");
    public static readonly CrowdfunderType Fundraiser = new("fundraiser", "Fundraiser");

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
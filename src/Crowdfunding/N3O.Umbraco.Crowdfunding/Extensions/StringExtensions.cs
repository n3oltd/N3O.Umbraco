using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class StringExtensions {
    public static CrowdfunderType ToCrowdfunderType(this string contentTypeAlias) {
        if (contentTypeAlias.EqualsInvariant(CrowdfundingConstants.Campaign.Alias)) {
            return CrowdfunderTypes.Campaign;
        } else if (contentTypeAlias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias)) {
            return CrowdfunderTypes.Fundraiser;
        } else {
            throw UnrecognisedValueException.For(contentTypeAlias);
        }
    }
}
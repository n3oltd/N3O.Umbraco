using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Exceptions;
using System;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class GuidExtensions {
    public static ICrowdfunderContent GetCrowdfunderContent(this IContentLocator contentLocator,
                                                            Guid id,
                                                            CrowdfunderType type) {
        if (type == CrowdfunderTypes.Campaign) {
            return contentLocator.ById<CampaignContent>(id);
        } else if (type == CrowdfunderTypes.Fundraiser) {
            return contentLocator.ById<FundraiserContent>(id);
        } else {
            throw UnrecognisedValueException.For(type);
        }
    }
}
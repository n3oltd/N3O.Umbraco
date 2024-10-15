using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Exceptions;
using System;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class GuidExtensions {
    public static ICrowdfunderContent ToCrowdfunderContent(this Guid id,
                                                           IContentLocator contentLocator,
                                                           CrowdfunderType crowdfunderType) {
        if (crowdfunderType == CrowdfunderTypes.Campaign) {
            return contentLocator.ById<CampaignContent>(id);
        } else if (crowdfunderType == CrowdfunderTypes.Fundraiser) {
            return contentLocator.ById<FundraiserContent>(id);
        } else {
            throw UnrecognisedValueException.For(crowdfunderType);
        }
    }
}
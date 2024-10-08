using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Json;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Extensions; 

public static class AllocationExtensions {
    public static CrowdfunderData GetCrowdfunderData(this IAllocation allocation, IJsonProvider jsonProvider) {
        if (!HasCrowdfunderData(allocation)) {
            return null;
        }

        return allocation.Extensions.Get<CrowdfunderData>(jsonProvider, Allocations.Extensions.Key);
    }
    
    public static bool HasCrowdfunderData(this IAllocation allocation) {
        return allocation.Extensions?.ContainsKey(Allocations.Extensions.Key) == true;
    }

    public static string GetCrowdfunderName(this IAllocation allocation, IJsonProvider jsonProvider, IContentLocator contentLocator) {
        if (!HasCrowdfunderData(allocation)) {
            return null;
        }
        
        var extensionData = allocation.Extensions.Get<CrowdfunderData>(jsonProvider, Allocations.Extensions.Key);
        
        ICrowdfunderContent crowdfunderContent;
        
        if (extensionData.CrowdfunderType == CrowdfunderTypes.Campaign) {
            crowdfunderContent = contentLocator.ById<CampaignContent>(extensionData.CrowdfunderId);
        } else if (extensionData.CrowdfunderType == CrowdfunderTypes.Fundraiser) {
            crowdfunderContent = contentLocator.ById<FundraiserContent>(extensionData.CrowdfunderId);
        } else {
            throw UnrecognisedValueException.For(extensionData.CrowdfunderType);
        }

        return crowdfunderContent.Name;
    } 

}
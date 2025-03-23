using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class WebhookCartItemExtensions {
    public static CrowdfunderData GetCrowdfunderData(this WebhookCartItem cartItem, IJsonProvider jsonProvider) {
        if (!HasCrowdfunderData(cartItem)) {
            return null;
        }

        return cartItem.Extensions.Get<CrowdfunderData>(jsonProvider, CrowdfundingConstants.Allocations.Extensions.Key);
    }
    
    public static bool HasCrowdfunderData(this WebhookCartItem cartItem) {
        return cartItem.Extensions?.ContainsKey(CrowdfundingConstants.Allocations.Extensions.Key) == true;
    }
}
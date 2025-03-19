using N3O.Umbraco.Crowdfunding.Models;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class WebhookCartItemExtensions {
    public static bool HasCrowdfunderData(this WebhookCartItem webhookCartItem) {
        return webhookCartItem.Extensions?.ContainsKey(CrowdfundingConstants.WebhookCartItem.Extensions.Key) == true;
    }
}
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Crm.Engage.Extensions;

public static class SubscriptionIdExtensions {
    public static string GetSubscriptionNumber(this EntityId subscriptionId) {
        if (subscriptionId.HasValue()) {
            return subscriptionId.ToString().Substring(0, 8).TrimStart('0');
        }

        return null;
    }
}
using N3O.Umbraco.Cloud.Models;

namespace N3O.Umbraco.Cloud;

public interface ISubscriptionAccessor {
    SubscriptionInfo GetSubscription();
}
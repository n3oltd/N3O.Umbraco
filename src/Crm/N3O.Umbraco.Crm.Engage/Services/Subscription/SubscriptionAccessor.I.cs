using N3O.Umbraco.Crm.Engage.Models;

namespace N3O.Umbraco.Crm.Engage;

public interface ISubscriptionAccessor {
    SubscriptionInfo GetSubscription();
}
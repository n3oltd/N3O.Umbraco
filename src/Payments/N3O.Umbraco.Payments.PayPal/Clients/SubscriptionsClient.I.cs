using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Clients;

public interface ISubscriptionsClient {
    [Post("/v1/billing/subscriptions")]
    Task<Subscription> GetSubscriptionAsync(string subscriptionId);
}
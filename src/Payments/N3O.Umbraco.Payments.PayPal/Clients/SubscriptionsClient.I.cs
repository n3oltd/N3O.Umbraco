using N3O.Umbraco.Payments.PayPal.Clients.PayPalErrors;
using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Clients;

public interface ISubscriptionsClient {
    [Post("/v1/billing/subscriptions")]
    Task<Subscription> CreateSubscriptionAsync(ApiCreateSubscription request);
}
using N3O.Umbraco.Webhooks.Lookups;
using System.Threading;

namespace N3O.Umbraco.Webhooks {
    public interface IWebhooks {
        void Queue(WebhookEvent webhookEvent, object body);
    }
}
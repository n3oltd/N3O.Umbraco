using N3O.Umbraco.Mediator;
using N3O.Umbraco.Webhooks.NamedParameters;

namespace N3O.Umbraco.Webhooks.Commands {
    public class QueueWebhookCommand : Request<None, None> {
        public WebhookEventId WebhookEventId { get; }
        public WebhookRoute WebhookRoute { get; }

        public QueueWebhookCommand(WebhookEventId webhookEventId, WebhookRoute webhookRoute) {
            WebhookEventId = webhookEventId;
            WebhookRoute = webhookRoute;
        }
    }
}
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Webhooks.NamedParameters;

namespace N3O.Umbraco.Webhooks.Commands {
    public class QueueWebhookCommand : Request<None, None> {
        public HookId HookId { get; }
        public HookRoute HookRoute { get; }

        public QueueWebhookCommand(HookId hookId, HookRoute hookRoute) {
            HookId = hookId;
            HookRoute = hookRoute;
        }
    }
}
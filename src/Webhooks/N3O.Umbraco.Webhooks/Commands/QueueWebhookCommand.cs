using N3O.Umbraco.Mediator;
using N3O.Umbraco.Webhooks.NamedParameters;

namespace N3O.Umbraco.Webhooks.Commands {
    public class QueueWebhookCommand : Request<None, None> {
        public EndpointId EndpointId { get; }
        public EndpointRoute EndpointRoute { get; }

        public QueueWebhookCommand(EndpointId endpointId, EndpointRoute endpointRoute) {
            EndpointId = endpointId;
            EndpointRoute = endpointRoute;
        }
    }
}
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Webhooks.Models;

namespace N3O.Umbraco.Webhooks.Commands;

public class ProcessWebhookCommand : Request<WebhookPayload, None> { }

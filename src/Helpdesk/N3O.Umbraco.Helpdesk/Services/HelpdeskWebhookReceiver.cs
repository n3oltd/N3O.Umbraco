using N3O.Umbraco.Content;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Models;
using N3O.Umbraco.Webhooks.Receivers;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Helpdesk.Services;

[WebhookReceiver(HookIds.DonationItem)]
public abstract class HelpdeskWebhookReceiver : WebhookReceiver {
    private readonly IContentLocator _contentLocator;

    public HelpdeskWebhookReceiver(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }

    protected Task ResolveHelpdesk();
    
    protected Task EnqueueAsync() {
        _contentLocator.All<GitHubHelpdeskContent>();

        // Write to Umbraco DB the basic metadata
        // Create an issue on GitHub with the specified ID
    }
}
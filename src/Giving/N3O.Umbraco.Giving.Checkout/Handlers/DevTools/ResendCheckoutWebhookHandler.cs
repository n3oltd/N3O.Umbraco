using N3O.Umbraco.Entities;
using N3O.Umbraco.Giving.Checkout.Commands;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Webhooks;
using N3O.Umbraco.Webhooks.Lookups;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout.Handlers.DevTools;

public class ResendCheckoutWebhookHandler : IRequestHandler<ResendCheckoutWebhookCommand, None, None> {
    private readonly IRepository<Entities.Checkout> _repository;
    private readonly IWebhooks _webhooks;
    
    public ResendCheckoutWebhookHandler(IRepository<Entities.Checkout> repository, IWebhooks webhooks) {
        _repository = repository;
        _webhooks = webhooks;
    }
    
    public async Task<None> Handle(ResendCheckoutWebhookCommand req, CancellationToken cancellationToken) {
        var checkout = await req.CheckoutRevisionId.RunAsync(_repository.GetAsync, true, cancellationToken);

        if (checkout.Donation.IsComplete) {
            SendWebhook(CheckoutWebhookEvents.DonationCompleteEvent, checkout);
        }
        
        if (checkout.RegularGiving.IsComplete) {
            SendWebhook(CheckoutWebhookEvents.RegularGivingCompleteEvent, checkout);
        }
        
        return None.Empty;
    }
    
    private void SendWebhook(WebhookEvent webhookEvent, Entities.Checkout checkout) {
        _webhooks.Queue(webhookEvent, checkout);
    }
}
using N3O.Umbraco.Webhooks.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Webhooks.Endpoints {
    public interface IWebhookEndpoint {
        Task HandleAsync(ReceivedWebhook webhook, CancellationToken cancellationToken);
    }
}
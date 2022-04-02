using N3O.Umbraco.Webhooks.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Webhooks.Receivers {
    public interface IWebhookReceiver {
        Task HandleAsync(Payload payload, CancellationToken cancellationToken);
    }
}
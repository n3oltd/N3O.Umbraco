using N3O.Umbraco.Webhooks.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Webhooks.Receivers {
    public abstract class WebhookReceiver : IWebhookReceiver {
        public async Task HandleAsync(WebhookPayload payload, CancellationToken cancellationToken) {
            var isAuthorised = await AuthoriseAsync(payload);

            if (!isAuthorised) {
                throw new Exception($"Authorisation failed for hook ID {payload.HookId}");
            }

            await ProcessAsync(payload, cancellationToken);
        }

        protected abstract Task ProcessAsync(WebhookPayload payload, CancellationToken cancellationToken);

        protected virtual Task<bool> AuthoriseAsync(WebhookPayload payload) => Task.FromResult(true);
    }
}
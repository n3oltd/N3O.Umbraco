using N3O.Umbraco.Json;
using N3O.Umbraco.Webhooks.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Webhooks.Receivers {
    public abstract class WebhookReceiver<TModel> : IWebhookReceiver {
        private readonly IJsonProvider _jsonProvider;

        protected WebhookReceiver(IJsonProvider jsonProvider) {
            _jsonProvider = jsonProvider;
        }
        
        public async Task HandleAsync(Payload payload, CancellationToken cancellationToken) {
            var isAuthorised = await AuthoriseAsync(payload);

            if (!isAuthorised) {
                throw new Exception($"Authorisation failed for webhook event {payload.EventId}");
            }

            var model = _jsonProvider.DeserializeObject<TModel>(payload.Body);

            await HandleAsync(model, cancellationToken);
        }

        protected abstract Task HandleAsync(TModel model, CancellationToken cancellationToken);

        protected virtual Task<bool> AuthoriseAsync(Payload payload) => Task.FromResult(true);
    }
}
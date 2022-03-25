using N3O.Umbraco.Json;
using N3O.Umbraco.Webhooks.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Webhooks.Endpoints {
    public abstract class WebhookEndpoint<TModel> : IWebhookEndpoint {
        private readonly IJsonProvider _jsonProvider;

        protected WebhookEndpoint(IJsonProvider jsonProvider) {
            _jsonProvider = jsonProvider;
        }
        
        public async Task HandleAsync(ReceivedWebhook webhook, CancellationToken cancellationToken) {
            var isAuthorised = await AuthoriseAsync();

            if (!isAuthorised) {
                throw new Exception($"Authorisation failed for webhook endpoint {webhook.EndpointId}");
            }

            var model = _jsonProvider.DeserializeObject<TModel>(webhook.Body);

            await HandleAsync(model, cancellationToken);
        }

        protected abstract Task HandleAsync(TModel model, CancellationToken cancellationToken);

        protected virtual Task<bool> AuthoriseAsync() => Task.FromResult(true);
    }
}
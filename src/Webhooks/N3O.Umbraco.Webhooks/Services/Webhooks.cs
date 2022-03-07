using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout;
using N3O.Umbraco.Json;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Utilities;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Content;
using N3O.Umbraco.Webhooks.Lookups;
using N3O.Umbraco.Webhooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Webhooks {
    public class Webhooks : IWebhooks {
        private readonly IContentCache _contentCache;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Lazy<IBackgroundJob> _backgroundJob;
        private readonly IJsonProvider _jsonProvider;

        public Webhooks(IContentCache contentCache,
                        IWebHostEnvironment webHostEnvironment,
                        Lazy<IBackgroundJob> backgroundJob,
                        IJsonProvider jsonProvider) {
            _contentCache = contentCache;
            _webHostEnvironment = webHostEnvironment;
            _backgroundJob = backgroundJob;
            _jsonProvider = jsonProvider;
        }

        public void Queue(WebhookEvent webhookEvent, object body) {
            var webhooks = GetWebhooks(webhookEvent);

            var transform = GetWebhookTransform(body.GetType());
            if (transform.HasValue()) {
                body = transform.CallMethod(nameof(IWebhookTransform<Entity>.Transform))
                                .WithParameter(typeof(IJsonProvider), _jsonProvider)
                                .WithParameter(body.GetType(), body)
                                .Run();
            }

            foreach (var webhook in webhooks) {
                var jobName = $"WH {webhookEvent.Name} to {webhook.Url}";

                var dispatchReq = new DispatchWebhookReq();
                dispatchReq.Url = webhook.Url;
                dispatchReq.Body = body;

                _backgroundJob.Value.Enqueue<DispatchWebhookCommand, DispatchWebhookReq>(jobName, dispatchReq);
            }
        }

        private IReadOnlyList<WebhookElement> GetWebhooks(WebhookEvent webhookEvent) {
            var settings = _contentCache.Single<WebhookSettingsContent>();

            var list = _webHostEnvironment.IsProduction() ? settings.OrEmpty(x => x.Production).ToList() : settings.OrEmpty(x => x.Staging).ToList();

            list.RemoveWhere(x => x.Event != webhookEvent);

            return list;
        }

        private object GetWebhookTransform(Type entityType) {
            var implementedType = OurAssemblies.GetTypes(t => t.ImplementsInterface(typeof(IWebhookTransform<>).MakeGenericType(entityType)) &&
                                                              !t.IsAbstract).SingleOrDefault();

            if (implementedType.HasValue()) {
                var transform = Activator.CreateInstance(implementedType);

                return transform;
            }

            return null;
        }
    }
}
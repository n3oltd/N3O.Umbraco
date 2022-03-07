using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Webhooks;
using N3O.Umbraco.Scheduler;
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
        private readonly IReadOnlyList<IWebhookTransform> _transforms;

        public Webhooks(IContentCache contentCache,
                        IWebHostEnvironment webHostEnvironment,
                        Lazy<IBackgroundJob> backgroundJob,
                        IEnumerable<IWebhookTransform> transforms) {
            _contentCache = contentCache;
            _webHostEnvironment = webHostEnvironment;
            _backgroundJob = backgroundJob;
            _transforms = transforms.OrEmpty().ApplyAttributeOrdering();
        }

        public void Queue(WebhookEvent webhookEvent, object body) {
            var webhooks = GetWebhooks(webhookEvent);

            body = ApplyTransforms(body);

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

            var list = _webHostEnvironment.IsProduction()
                           ? settings.OrEmpty(x => x.Production).ToList()
                           : settings.OrEmpty(x => x.Staging).ToList();

            list.RemoveWhere(x => x.Event != webhookEvent);

            return list;
        }

        private object ApplyTransforms(object body) {
            var transforms = _transforms.Where(x => x.IsTransform(body)).ToList();

            foreach (var transform in transforms) {
                body = transform.Apply(body);
            }

            return body;
        }
    }
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
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

        public Webhooks(IContentCache contentCache,
                        IWebHostEnvironment webHostEnvironment,
                        Lazy<IBackgroundJob> backgroundJob) {
            _contentCache = contentCache;
            _webHostEnvironment = webHostEnvironment;
            _backgroundJob = backgroundJob;
        }
        
        public void Queue(WebhookEvent webhookEvent, object body) {
            var webhooks = GetWebhooks(webhookEvent);

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

            var list = _webHostEnvironment.IsProduction() ?
                           settings.OrEmpty(x => x.Production).ToList() :
                           settings.OrEmpty(x => x.Staging).ToList();

            list.RemoveWhere(x => x.Event != webhookEvent);

            return list;
        }
    }
}
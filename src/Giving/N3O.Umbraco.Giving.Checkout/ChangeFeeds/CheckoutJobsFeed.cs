using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Email;
using N3O.Umbraco.Email.Content;
using N3O.Umbraco.Email.Extensions;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Commands;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Webhooks;
using NodaTime;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout.ChangeFeeds {
    public partial class CheckoutJobsFeed : ChangeFeed<Entities.Checkout> {
        private readonly Lazy<IWebhooks> _webhooks;
        private readonly Lazy<IEmailBuilder> _emailBuilder;
        private readonly Lazy<IContentCache> _contentCache;
        private readonly Lazy<IBackgroundJob> _backgroundJob;

        public CheckoutJobsFeed(ILogger<CheckoutJobsFeed> logger,
                                Lazy<IWebhooks> webhooks,
                                Lazy<IEmailBuilder> emailBuilder,
                                Lazy<IContentCache> contentCache,
                                Lazy<IBackgroundJob> backgroundJob)
            : base(logger) {
            _webhooks = webhooks;
            _emailBuilder = emailBuilder;
            _contentCache = contentCache;
            _backgroundJob = backgroundJob;
        }
        
        protected override Task ProcessAsync(EntityChange<Entities.Checkout> entityChange,
                                             CancellationToken cancellationToken) {
            ProcessDonationJobs(entityChange);
            //ProcessCompleteJobs(entityChange);
            
            return Task.CompletedTask;
        }

        private void ScheduleJob<TCommand>(Entities.Checkout checkout, Duration fromNow)
            where TCommand : CheckoutJobCommand {
            _backgroundJob.Value.Schedule<TCommand, Entities.Checkout>($"{typeof(TCommand).GetFriendlyName()}({checkout.Reference})",
                                                                       fromNow,
                                                                       checkout);
        }
        
        private void SendEmail<T>(Entities.Checkout checkout) where T : EmailTemplateContent<T> {
            var template = _contentCache.Value.Single<T>();

            if (template.HasValue() && checkout.HasValue(x => x.Account?.Email?.Address)) {
                _emailBuilder.Value.SendTemplate(template, checkout.Account.Email.Address, checkout);
            }
        }
    }
}
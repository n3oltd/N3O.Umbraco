using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Email;
using N3O.Umbraco.Email.Content;
using N3O.Umbraco.Email.Extensions;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Cart.Commands;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Cart.NamedParameters;
using N3O.Umbraco.Giving.Checkout.Commands;
using N3O.Umbraco.Giving.Checkout.Content;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Webhooks;
using N3O.Umbraco.Webhooks.Lookups;
using NodaTime;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Checkout.ChangeFeeds;

/*
 * We can define an interface (along the lines of, don't take too literally)
 * ICheckoutNotificationHandler which we can inject the IEnumerable<> of into
 * this class. We can then call those notifications as required. We can keep this
 * general or simplify it by keeping it very specific, e.g. CrowdFundingCheckoutCompleteHandler
 * which is passed the completed checkout object, looks into the allocation and copies
 * the relevant data into the SQL table.
 *
 * In some sense this is redundant in that our system already supports multiple change feeds
 * so we could argue it is easier just to add a changefeed<checkout> rather than another layer
 * of abstraction. The only advantage of a more specialised interface is we don't distribute
 * the meaning of when a checkout is complete and also we can ensure that critical steps complete
 * before the less essential ones.
 */

public class CheckoutFeed : ChangeFeed<Entities.Checkout> {
    private readonly Lazy<IWebhooks> _webhooks;
    private readonly Lazy<IEmailBuilder> _emailBuilder;
    private readonly Lazy<IContentCache> _contentCache;
    private readonly Lazy<IBackgroundJob> _backgroundJob;

    public CheckoutFeed(ILogger<CheckoutFeed> logger,
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
    
    protected override Task ProcessChangeAsync(EntityChange<Entities.Checkout> entityChange) {
        if (entityChange.Operation == EntityOperations.Update) {
            Process(entityChange, x => x.Donation.IsComplete, c => {
                SendEmail<DonationReceiptTemplateContent>(c);
                SendWebhook(CheckoutWebhookEvents.DonationCompleteEvent, c);
                ClearCart(GivingTypes.Donation, c.CartRevisionId.Id);
            });

            Process(entityChange, x => x.RegularGiving.IsComplete, c => {
                SendEmail<RegularGivingReceiptTemplateContent>(c);
                SendWebhook(CheckoutWebhookEvents.RegularGivingCompleteEvent, c);
                ClearCart(GivingTypes.RegularGiving, c.CartRevisionId.Id);
            });

            Process(entityChange, x => x.IsComplete, c => {
                ScheduleJob<DeleteCheckoutCommand>(c, Duration.FromDays(90));
            });
        }

        return Task.CompletedTask;
    }

    private void Process(EntityChange<Entities.Checkout> entityChange,
                         Func<Entities.Checkout, bool> getComplete,
                         Action<Entities.Checkout> action) {
        var isComplete = getComplete(entityChange.SessionEntity);
        var wasComplete = getComplete(entityChange.DatabaseEntity);

        if (isComplete && !wasComplete) {
            var checkout = entityChange.SessionEntity;
            
            action(checkout);
        }
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
            _emailBuilder.Value.QueueTemplate(template, checkout.Account.Email.Address, checkout);
        }
    }
    
    private void SendWebhook(WebhookEvent webhookEvent, Entities.Checkout checkout) {
        _webhooks.Value.Queue(webhookEvent, checkout);
    }
    
    private void ClearCart(GivingType givingType, EntityId cartId) {
        var jobName = $"{nameof(ClearCartCommand)}({givingType.Id}, {cartId})";
        
        var req = new ClearCartReq();
        req.GivingType = givingType;

        _backgroundJob.Value.Enqueue<ClearCartCommand, ClearCartReq>(jobName,
                                                                     req,
                                                                     p => p.Add<CartId>(cartId));
    }
}

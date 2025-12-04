using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Scheduler;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class OfferingUnpublished : CloudContentUnpublished {
    private readonly IContentTypeService _contentTypeService;

    public OfferingUnpublished(ISubscriptionAccessor subscriptionAccessor,
                               IBackgroundJob backgroundJob,
                               IContentTypeService contentTypeService) : base(subscriptionAccessor, backgroundJob) {
        _contentTypeService = contentTypeService;
    }

    protected override Task<object> GetBody(IContent content) {
        var offeringReq = new OfferingWebhookBodyReq();
        offeringReq.Id = content.Key.ToString();
        offeringReq.Action = WebhookSyncAction.Deactivate;

        return Task.FromResult<object>(offeringReq);
    }

    protected override bool CanProcess(IContent content) {
        return content.IsOffering(_contentTypeService);
    }

    protected override string HookId => PlatformsConstants.Settings.WebhookIds.Offerings;
}
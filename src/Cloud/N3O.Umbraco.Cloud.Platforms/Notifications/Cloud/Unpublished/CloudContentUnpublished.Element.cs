using N3O.Umbraco.Cloud.Clients.Platforms;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Scheduler;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ElementUnpublished : CloudContentUnpublished {
    private readonly IContentTypeService _contentTypeService;

    public ElementUnpublished(ISubscriptionAccessor subscriptionAccessor,
                              ICloudUrl cloudUrl,
                              IBackgroundJob backgroundJob,
                              IContentTypeService contentTypeService)
        : base(subscriptionAccessor, cloudUrl, backgroundJob) {
        _contentTypeService = contentTypeService;
    }

    protected override object GetBody(IContent content) {
        var elementReq = new ElementWebhookBodyReq();
        elementReq.Id = content.Key.ToString();
        elementReq.Action = WebhookSyncAction.Deactivate;

        return elementReq;
    }

    protected override bool CanProcess(IContent content) {
        return content.IsElement(_contentTypeService);
    }

    protected override string HookId => PlatformsConstants.WebhookIds.Elements;
}
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Scheduler;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ElementDeleted : CloudContentDeleted {
    private readonly IContentTypeService _contentTypeService;

    public ElementDeleted(ISubscriptionAccessor subscriptionAccessor,
                          IBackgroundJob backgroundJob,
                          IContentTypeService contentTypeService) : base(subscriptionAccessor, backgroundJob) {
        _contentTypeService = contentTypeService;
    }

    protected override object GetBody(IContent content) {
        var elementReq = new ElementWebhookBodyReq();
        elementReq.Id = content.Key.ToString();
        elementReq.Action = WebhookSyncAction.Delete;

        return elementReq;
    }

    protected override bool CanProcess(IContent content) {
        return content.IsElement(_contentTypeService);
    }

    protected override string HookId => PlatformsConstants.Settings.WebhookIds.Elements;
}
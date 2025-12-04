using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Scheduler;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ElementUnpublished : CloudContentUnpublished {
    private readonly IContentTypeService _contentTypeService;

    public ElementUnpublished(ISubscriptionAccessor subscriptionAccessor,
                              IBackgroundJob backgroundJob,
                              IContentTypeService contentTypeService) : base(subscriptionAccessor, backgroundJob) {
        _contentTypeService = contentTypeService;
    }

    protected override Task<object> GetBody(IContent content) {
        var elementReq = new ElementWebhookBodyReq();
        elementReq.Id = content.Key.ToString();
        elementReq.Action = WebhookSyncAction.Deactivate;

        return Task.FromResult<object>(elementReq);
    }

    protected override bool CanProcess(IContent content) {
        return content.IsElement(_contentTypeService);
    }

    protected override string HookId => PlatformsConstants.Settings.WebhookIds.Elements;
}
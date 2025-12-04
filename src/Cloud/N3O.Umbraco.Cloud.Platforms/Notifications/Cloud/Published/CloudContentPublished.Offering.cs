using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class OfferingPublished : CloudContentPublished {
    private readonly IContentTypeService _contentTypeService;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public OfferingPublished(ISubscriptionAccessor subscriptionAccessor,
                             IBackgroundJob backgroundJob,
                             IContentTypeService contentTypeService,
                             Lazy<IContentLocator> contentLocator,
                             IUmbracoMapper mapper) : base(subscriptionAccessor, backgroundJob) {
        _contentTypeService = contentTypeService;
        _contentLocator = contentLocator;
        _mapper = mapper;
    }

    protected override Task<object> GetBody(IContent content) {
        var offering = _contentLocator.Value.ById<OfferingContent>(content.Key);

        var offeringReq = _mapper.Map<OfferingContent, OfferingWebhookBodyReq>(offering);

        return Task.FromResult<object>(offeringReq);
    }

    protected override bool CanProcess(IContent content) {
        return content.IsOffering(_contentTypeService);
    }

    protected override string HookId => PlatformsConstants.Settings.WebhookIds.Offerings;
}
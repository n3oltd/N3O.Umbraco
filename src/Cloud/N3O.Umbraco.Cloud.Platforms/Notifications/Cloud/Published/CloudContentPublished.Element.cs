using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ElementPublished : CloudContentPublished {
    private readonly IContentTypeService _contentTypeService;
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public ElementPublished(ISubscriptionAccessor subscriptionAccessor,
                            IBackgroundJob backgroundJob,
                            IContentTypeService contentTypeService,
                            Lazy<IContentLocator> contentLocator,
                            IUmbracoMapper mapper) : base(subscriptionAccessor, backgroundJob) {
        _contentTypeService = contentTypeService;
        _contentLocator = contentLocator;
        _mapper = mapper;
    }

    protected override object GetBody(IContent content) {
        var element = _contentLocator.Value.ById<ElementContent>(content.Key);

        var elementReq = _mapper.Map<ElementContent, ElementWebhookBodyReq>(element);

        return elementReq;
    }

    protected override bool CanProcess(IContent content) {
        return content.IsElement(_contentTypeService);
    }

    protected override string HookId => PlatformsConstants.Settings.WebhookIds.Elements;
}
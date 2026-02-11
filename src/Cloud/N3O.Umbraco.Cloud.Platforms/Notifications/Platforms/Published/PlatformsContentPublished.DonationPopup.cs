using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class DonationPopupPublished : CloudContentPublished {
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public DonationPopupPublished(ISubscriptionAccessor subscriptionAccessor,
                                  ICloudUrl cloudUrl,
                                  IBackgroundJob backgroundJob,
                                  Lazy<IContentLocator> contentLocator,
                                  IUmbracoMapper mapper,
                                  ILogger<CloudContentPublished> logger)
        : base(subscriptionAccessor, cloudUrl, backgroundJob, logger) {
        _contentLocator = contentLocator;
        _mapper = mapper;
    }
    
    protected override bool CanProcess(IContent content) {
        return content.IsDonationPopupElement();
    }

    protected override Task<object> GetBodyAsync(IContent content) {
        var donationPopup = _contentLocator.Value.ById<ElementContent>(content.Key);

        var donationPopupReq = _mapper.Map<ElementContent, CustomElementWebhookBodyReqDonationPopupReq>(donationPopup);

        return Task.FromResult<object>(donationPopupReq);
    }

    protected override string HookId => PlatformsConstants.WebhookIds.DonationPopups;
}
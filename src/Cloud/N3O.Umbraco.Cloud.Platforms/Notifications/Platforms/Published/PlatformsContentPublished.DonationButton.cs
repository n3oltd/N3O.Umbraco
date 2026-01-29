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

public class DonationButtonPublished : CloudContentPublished {
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public DonationButtonPublished(ISubscriptionAccessor subscriptionAccessor,
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
        return content.IsDonationButtonElement();
    }

    protected override Task<object> GetBodyAsync(IContent content) {
        var donationButton = _contentLocator.Value.ById<ElementContent>(content.Key);

        var donationButtonReq = _mapper.Map<ElementContent, CustomElementWebhookBodyReqDonationButtonReq>(donationButton);

        return Task.FromResult<object>(donationButtonReq);
    }

    protected override string HookId => PlatformsConstants.WebhookIds.DonationButtons;
}
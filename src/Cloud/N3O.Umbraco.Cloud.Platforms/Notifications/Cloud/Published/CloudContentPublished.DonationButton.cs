using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System;
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
                                   IUmbracoMapper mapper)
        : base(subscriptionAccessor, cloudUrl, backgroundJob) {
        _contentLocator = contentLocator;
        _mapper = mapper;
    }

    protected override object GetBody(IContent content) {
        var donationButton = _contentLocator.Value.ById<ElementContent>(content.Key);

        var donationButtonReq = _mapper.Map<ElementContent, CustomElementWebhookBodyReqDonationButtonReq>(donationButton);

        return donationButtonReq;
    }

    protected override bool CanProcess(IContent content) {
        return content.IsDonationButtonElement();
    }

    protected override string HookId => PlatformsConstants.WebhookIds.DonationButtons;
}
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Scheduler;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class DonationFormPublished : CloudContentPublished {
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IUmbracoMapper _mapper;

    public DonationFormPublished(ISubscriptionAccessor subscriptionAccessor,
                                 ICloudUrl cloudUrl,
                                 IBackgroundJob backgroundJob,
                                 Lazy<IContentLocator> contentLocator,
                                 IUmbracoMapper mapper,
                                 ILogger<CloudContentPublished> logger)
        : base(subscriptionAccessor, cloudUrl, backgroundJob, logger) {
        _contentLocator = contentLocator;
        _mapper = mapper;
    }

    protected override object GetBody(IContent content) {
        var donationForm = _contentLocator.Value.ById<ElementContent>(content.Key);

        var donationFormReq = _mapper.Map<ElementContent, CustomElementWebhookBodyReqDonationFormReq>(donationForm);

        return donationFormReq;
    }

    protected override bool CanProcess(IContent content) {
        return content.IsDonationFormElement();
    }

    protected override string HookId => PlatformsConstants.WebhookIds.DonationForms;
}
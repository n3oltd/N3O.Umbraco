using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Email;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

[Order(1)]
[SkipDuringSync]
public class FundraiserPublishing : INotificationAsyncHandler<ContentPublishingNotification> {
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly Lazy<IEmailBuilder> _emailBuilder;
    private readonly Lazy<ILocalClock> _localClock;

    public FundraiserPublishing(Lazy<IEmailBuilder> emailBuilder,
                                Lazy<IContentLocator> contentLocator,
                                Lazy<ILocalClock> localClock) {
        _emailBuilder = emailBuilder;
        _contentLocator = contentLocator;
        _localClock = localClock;
    }

    public Task HandleAsync(ContentPublishingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.FundraiserNotificationEmail.Alias)) {
                var resend = content.GetValue<bool>(CrowdfundingConstants.FundraiserNotificationEmail.Properties.Resend);
                var resendTo = content.GetValue<string>(CrowdfundingConstants.FundraiserNotificationEmail.Properties.ResendTo);

                if (resend && resendTo.HasValue()) {
                    EnqueueEmail(content, resendTo);

                    content.SetValue(CrowdfundingConstants.FundraiserNotificationEmail.Properties.Resend, false);
                    content.SetValue(CrowdfundingConstants.FundraiserNotificationEmail.Properties.SentAt, _localClock.Value.GetUtcNow());
                }
            }
        }

        return Task.CompletedTask;
    }

    private void EnqueueEmail(IContent content, string resendTo) {
        var fundraiser = _contentLocator.Value.ById<FundraiserContent>(content.ParentId);

        var subject = content.GetValue<string>(CrowdfundingConstants.FundraiserNotificationEmail.Properties.Subject);
        var body = content.GetValue<string>(CrowdfundingConstants.FundraiserNotificationEmail.Properties.Body);
        var fromEmail = content.GetValue<string>(CrowdfundingConstants.FundraiserNotificationEmail.Properties.FromEmail);
        var fromName = content.GetValue<string>(CrowdfundingConstants.FundraiserNotificationEmail.Properties.FromName);

        _emailBuilder.Value
                     .Create<FundraiserContent>()
                     .From(fromEmail, fromName)
                     .To(resendTo)
                     .Subject(subject)
                     .Body(body)
                     .Model(fundraiser)
                     .Queue();
    }
}
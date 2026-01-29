using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content.Templates;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Email;
using N3O.Umbraco.Email.Content;
using N3O.Umbraco.Email.Extensions;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingNotifications : ICrowdfundingNotifications {
    private readonly IContentCache _contentCache;
    private readonly IContentEditor _contentEditor;
    private readonly IEmailBuilder _emailBuilder;
    private readonly ILocalClock _localClock;

    public CrowdfundingNotifications(IContentCache contentCache,
                                     IContentEditor contentEditor,
                                     IEmailBuilder emailBuilder,
                                     ILocalClock localClock) {
        _contentCache = contentCache;
        _contentEditor = contentEditor;
        _emailBuilder = emailBuilder;
        _localClock = localClock;
    }

    public void Enqueue(FundraiserNotificationType notificationType,
                        FundraiserNotificationViewModel model,
                        Guid contentKey) {
        if (notificationType == FundraiserNotificationTypes.FundraiserCreated) {
            SendEmail<FundraiserCreatedTemplateContent>(notificationType, model, contentKey);
        } else if (notificationType == FundraiserNotificationTypes.StillDraft) {
            SendEmail<FundraiserDraftTemplateContent>(notificationType, model, contentKey);
        } else if (notificationType == FundraiserNotificationTypes.GoalsCompleted) {
            SendEmail<FundraiserGoalsCompletedTemplateContent>(notificationType, model, contentKey);
        } else if (notificationType == FundraiserNotificationTypes.GoalsExceeded) {
            SendEmail<FundraiserGoalsExceededTemplateContent>(notificationType, model, contentKey);
        } else if (notificationType == FundraiserNotificationTypes.FundraiserAbandoned) {
            SendEmail<FundraiserAbandonedTemplateContent>(notificationType, model, contentKey);
        } else {
            throw UnrecognisedValueException.For(notificationType);
        }
    }

    private void SendEmail<T>(FundraiserNotificationType notificationType,
                              FundraiserNotificationViewModel model,
                              Guid contentKey) 
        where T : EmailTemplateContent<T> {
        var template = _contentCache.Single<T>();

        if (template.HasValue()) {
            _emailBuilder.QueueTemplate(template, model.Fundraiser.FundraiserEmail, model);
            
            AddNotificationToFundraiserContent(notificationType, model, template, contentKey);
        }
    }
    
    private void AddNotificationToFundraiserContent<T>(FundraiserNotificationType notificationType,
                                                       FundraiserNotificationViewModel model,
                                                       EmailTemplateContent<T> template,
                                                       Guid contentKey) 
        where T : EmailTemplateContent<T> {
        var now = _localClock.GetUtcNow();
        
        var contentPublisher = _contentEditor.New($"{notificationType.Name} Email", contentKey, CrowdfundingConstants.FundraiserNotificationEmail.Alias);
        
        contentPublisher.Content
                        .Label(CrowdfundingConstants.FundraiserNotificationEmail.Properties.Type)
                        .Set(notificationType.Name);
        
        contentPublisher.Content
                        .Label(CrowdfundingConstants.FundraiserNotificationEmail.Properties.To)
                        .Set(model.Fundraiser.FundraiserEmail);

        contentPublisher.Content
                        .Label(CrowdfundingConstants.FundraiserNotificationEmail.Properties.SentAt)
                        .Set(now);
        
        contentPublisher.Content
                        .Label(CrowdfundingConstants.FundraiserNotificationEmail.Properties.Subject)
                        .Set(template.Subject);
        
        contentPublisher.Content
                        .TemplatedLabel(CrowdfundingConstants.FundraiserNotificationEmail.Properties.Body)
                        .Set(template.Body);
        
        contentPublisher.Content
                        .TemplatedLabel(CrowdfundingConstants.FundraiserNotificationEmail.Properties.FromEmail)
                        .Set(template.FromEmail);
        
        contentPublisher.Content
                        .TemplatedLabel(CrowdfundingConstants.FundraiserNotificationEmail.Properties.FromName)
                        .Set(template.FromName);

        var result = contentPublisher.SaveAndPublish();

        if (!result.Success) {
            throw new Exception("Failed to publish email content");
        }
    }
}

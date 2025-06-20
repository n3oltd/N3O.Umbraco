using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

[RecurringJob("Notify Draft Fundraisers", "0 0 * * 0")]
public class NotifyDraftFundraisersHandler : IRequestHandler<NotifyDraftFundraisersCommand, None, None> {
    private static readonly List<TimeSpan> Intervals = [TimeSpan.FromDays(7), TimeSpan.FromDays(14)];
    
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingNotifications _crowdfundingNotifications;
    private readonly ILocalClock _localClock;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly ICrowdfundingUrlBuilder _crowdfundingUrlBuilder;

    public NotifyDraftFundraisersHandler(IContentLocator contentLocator,
                                         ICrowdfundingNotifications crowdfundingNotifications,
                                         ILocalClock localClock,
                                         IUmbracoDatabaseFactory umbracoDatabaseFactory,
                                         ICrowdfundingUrlBuilder crowdfundingUrlBuilder) {
        _contentLocator = contentLocator;
        _crowdfundingNotifications = crowdfundingNotifications;
        _localClock = localClock;
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
    }

    public async Task<None> Handle(NotifyDraftFundraisersCommand req, CancellationToken cancellationToken) {
        List<Crowdfunder> crowdfunders;
        
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var query = db.Query<Crowdfunder>();
            
            query.Where(x => x.Type == CrowdfunderTypes.Fundraiser.Key);
            query.Where(x => x.CreatedAt < _localClock.GetCurrentInstant().ToDateTimeUtc().AddDays(-30));
            query.Where(x => x.StatusKey == CrowdfunderStatuses.Draft.Key);
            
            crowdfunders = await query.ToListAsync();
        }
        
        foreach (var crowdfunder in crowdfunders) {
            var fundraiserContent = _contentLocator.ById<FundraiserContent>(crowdfunder.ContentKey);
            
            var sentNotifications = _contentLocator.All<FundraiserNotificationEmailContent>(x => x.Content()?.Parent?.Key == fundraiserContent.Key &&
                                                                                                 x.Type.EqualsInvariant(FundraiserNotificationTypes.StillDraft.Name));

             if (ShouldSend(sentNotifications, crowdfunder.CreatedAt)) {
                SendEmail(fundraiserContent);
            }
        }

        return None.Empty;
    }
    
    private bool ShouldSend(IReadOnlyList<FundraiserNotificationEmailContent> sentNotifications, DateTime createdAt) {
        var currentDate = DateTime.UtcNow;
        
        foreach (var interval in Intervals) {
            var nextDueDate = createdAt.Add(interval);
            
            if (currentDate >= nextDueDate &&
                sentNotifications.All(x => nextDueDate > x.SentAt) &&
                nextDueDate >= createdAt.Add(interval)) {
                return true;
            }
        }

        return false;
    }
    
    private void SendEmail(FundraiserContent fundraiser) {
        var fundraiserContentViewModel = new FundraiserContentViewModel(_crowdfundingUrlBuilder, fundraiser);
        var model = new FundraiserNotificationViewModel(fundraiserContentViewModel, null);

        _crowdfundingNotifications.Enqueue(FundraiserNotificationTypes.StillDraft ,model, fundraiser.Key);
    }
}
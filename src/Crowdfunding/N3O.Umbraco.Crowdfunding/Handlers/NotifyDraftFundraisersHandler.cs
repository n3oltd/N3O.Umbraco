using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
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
    private readonly List<int> Intervals = new() { 7, 14 };
    
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingNotifications _crowdfundingNotifications;
    private readonly ILocalClock _localClock;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;

    public NotifyDraftFundraisersHandler(IContentLocator contentLocator,
                                         ICrowdfundingNotifications crowdfundingNotifications,
                                         ILocalClock localClock,
                                         IUmbracoDatabaseFactory umbracoDatabaseFactory) {
        _contentLocator = contentLocator;
        _crowdfundingNotifications = crowdfundingNotifications;
        _localClock = localClock;
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
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
    
    private bool ShouldSend(IReadOnlyList<FundraiserNotificationEmailContent> sentNotifications,
                            DateTime createdAt) {
        var currentDate = DateTime.UtcNow;
        
        for (int i = 0; i < Intervals.Count; i++) {
            var nextDueDate = createdAt.AddDays(Intervals[i]);
            
            if (currentDate >= nextDueDate && sentNotifications.All(x => nextDueDate > x.SentAt) && nextDueDate >= createdAt.AddDays(Intervals[i])) {
                return true;
            }
        }

        return false;
    }
    
    private void SendEmail(FundraiserContent fundraiser) {
        var fundraiserContentViewModel = new FundraiserContentViewModel(fundraiser);
        var model = new FundraiserNotificationViewModel(fundraiserContentViewModel, null);

        _crowdfundingNotifications.Enqueue(FundraiserNotificationTypes.StillDraft ,model, fundraiser.Key);
    }
}
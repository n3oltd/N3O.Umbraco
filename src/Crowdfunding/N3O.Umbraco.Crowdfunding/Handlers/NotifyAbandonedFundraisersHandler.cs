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

[RecurringJob("Notify Abandoned Fundraisers", "0 0 * * 0")]
public class NotifyAbandonedFundraisersHandler : IRequestHandler<NotifyAbandonedFundraisersCommand, None, None> {
    private static readonly List<int> MonthlyIntervals = [1, 3, 6];
    
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingNotifications _crowdfundingNotifications;
    private readonly ILocalClock _localClock;
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly ICrowdfundingUrlBuilder _crowdfundingUrlBuilder;

    public NotifyAbandonedFundraisersHandler(IContentLocator contentLocator,
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

    public async Task<None> Handle(NotifyAbandonedFundraisersCommand req, CancellationToken cancellationToken) {
        List<Crowdfunder> crowdfunders;

        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var query = db.Query<Crowdfunder>();
            
            query.Where(x => x.Type == CrowdfunderTypes.Fundraiser.Key);
            query.Where(x => x.LastContributionOn < _localClock.GetCurrentInstant().ToDateTimeUtc().AddDays(-30));
            query.Where(x => x.StatusKey == CrowdfunderStatuses.Active.Key);
            
            crowdfunders = await query.ToListAsync();
        }

        foreach (var crowdfunder in crowdfunders) {
            var fundraiserContent = _contentLocator.ById<FundraiserContent>(crowdfunder.ContentKey);
            
            var sentNotifications = _contentLocator.All<FundraiserNotificationEmailContent>(x => x.Content()?.Parent?.Key == fundraiserContent.Key &&
                                                                                                 x.Type.EqualsInvariant(FundraiserNotificationTypes.FundraiserAbandoned.Name));

            if (ShouldSend(sentNotifications, crowdfunder.LastContributionOn)) {
                SendEmail(fundraiserContent);
            }
        }

        return None.Empty;
    }
    
    private bool ShouldSend(IReadOnlyList<FundraiserNotificationEmailContent> sentNotifications,
                            DateTime lastContributionOn) {
        var currentDate = DateTime.UtcNow;
        
        foreach (var interval in MonthlyIntervals) {
            var nextDueDate = lastContributionOn.AddMonths(interval);
            
            if (currentDate >= nextDueDate &&
                sentNotifications.All(x => nextDueDate > x.SentAt) &&
                nextDueDate >= lastContributionOn.AddMonths(interval)) {
                return true;
            }
        }

        return false;
    }
    
    private void SendEmail(FundraiserContent fundraiser) {
        var fundraiserContentViewModel = new FundraiserContentViewModel(_crowdfundingUrlBuilder, fundraiser);
        var model = new FundraiserNotificationViewModel(fundraiserContentViewModel, null);

        _crowdfundingNotifications.Enqueue(FundraiserNotificationTypes.FundraiserAbandoned, model, fundraiser.Key);
    }
}
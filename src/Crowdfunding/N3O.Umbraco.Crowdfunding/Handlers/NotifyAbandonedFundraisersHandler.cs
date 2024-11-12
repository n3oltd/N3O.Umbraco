using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Attributes;
using NodaTime;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

//TODO Update time interval
[RecurringJob("Notify Abandoned Fundraisers", "0 0 * * 0")]
public class NotifyAbandonedFundraisersHandler : IRequestHandler<NotifyAbandonedFundraisersCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly IBackgroundJob _backgroundJob;
    private readonly ILocalClock _localClock;

    public NotifyAbandonedFundraisersHandler(IContentLocator contentLocator,
                                             ILocalClock localClock,
                                             IBackgroundJob backgroundJob) {
        _contentLocator = contentLocator;
        _localClock = localClock;
        _backgroundJob = backgroundJob;
    }

    public Task<None> Handle(NotifyAbandonedFundraisersCommand req, CancellationToken cancellationToken) {
        var allFundraisers = _contentLocator.All<FundraiserContent>();
        var thirtyDaysAgo = _localClock.GetLocalNow().Minus(Period.FromDays(15)).ToDateTimeUnspecified();
        
        var toNotify = allFundraisers.Where(x => (!x.Status.HasValue() || x.Status == CrowdfunderStatuses.Draft) &&
                                                 x.Content().UpdateDate < thirtyDaysAgo);
        
        toNotify.Do(SendEmail);

        return Task.FromResult(None.Empty);
    }
    
    private void SendEmail(FundraiserContent fundraiser) {
        var req = new FundraiserNotificationReq();
        req.Type = FundraiserNotificationTypes.FundraiserAbandoned;
        req.Fundraiser = new FundraiserNotificationViewModel(fundraiser);
            
        _backgroundJob.Enqueue<SendFundraiserNotificationCommand, FundraiserNotificationReq>($"Send{req.Type.Name}Email",
                                                                                             req);
    }
}
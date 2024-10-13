using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Events;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public abstract class PledgeEventHandler<TEvent> : IRequestHandler<TEvent, WebhookPledge, None> 
    where TEvent : PledgeEvent {
    private readonly AsyncKeyedLocker<string> _asyncKeyedLocker;
    private readonly IContentLocator _contentLocator;
    private readonly IServiceProvider _serviceProvider;
    
    protected PledgeEventHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                 IContentLocator contentLocator,
                                 IServiceProvider serviceProvider) {
        _contentLocator = contentLocator;
        _serviceProvider = serviceProvider;
        _asyncKeyedLocker = asyncKeyedLocker;
    }

    public async Task<None> Handle(TEvent req, CancellationToken cancellationToken) {
        using (await _asyncKeyedLocker.LockAsync(req.Model.Revision.Id.ToString(), cancellationToken)) {
            await HandleEventAsync(req, cancellationToken);
        }

        return None.Empty;
    }
    
    protected ICrowdfunderContent GetCrowdfunderContent(ICrowdfunderInfo crowdfunderInfo) {
        if (crowdfunderInfo.Type == CrowdfunderTypes.Campaign) {
            return _contentLocator.ById<CampaignContent>(crowdfunderInfo.Id);
        } else if (crowdfunderInfo.Type == CrowdfunderTypes.Fundraiser) {
            return _contentLocator.ById<FundraiserContent>(crowdfunderInfo.Id);
        } else {
            throw UnrecognisedValueException.For(crowdfunderInfo.Type);
        }
    }
    
    protected abstract Task HandleEventAsync(TEvent req, CancellationToken cancellationToken);
}
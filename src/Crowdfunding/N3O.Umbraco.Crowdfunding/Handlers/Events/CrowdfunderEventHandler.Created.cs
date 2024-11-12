using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Scheduler;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderCreatedHandler : CrowdfunderEventHandler<CrowdfunderCreatedEvent> {
    private readonly IContentLocator _contentLocator;
    private readonly IBackgroundJob _backgroundJob;

    public CrowdfunderCreatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                     IContentService contentService,
                                     IContentLocator contentLocator,
                                     ICrowdfunderRevisionRepository crowdfunderRevisionRepository,
                                     IBackgroundJob backgroundJob)
        : base(asyncKeyedLocker, contentService, contentLocator, crowdfunderRevisionRepository) {
        _contentLocator = contentLocator;
        _backgroundJob = backgroundJob;
    }

    protected override async Task HandleEventAsync(CrowdfunderCreatedEvent req, CancellationToken cancellationToken) {
        var content = GetContent(req.Model.Id);
        var type = content.ContentType.Alias.ToCrowdfunderType();

        await AddOrUpdateRevisionAsync(content.Key, content.VersionId, type);

        UpdateAndPublishStatus(content, req.Model.Status.Name);

        if (type == CrowdfunderTypes.Fundraiser) {
            SendFundraiserCreatedEmail(content);
        }
    }

    private void SendFundraiserCreatedEmail(IContent content) {
        var fundraiser = _contentLocator.ById<FundraiserContent>(content.Key);
        
        var req = new FundraiserNotificationReq();
        req.Type = FundraiserNotificationTypes.StillDraft;
        req.Fundraiser = new FundraiserNotificationViewModel(fundraiser);
            
        _backgroundJob.Enqueue<SendFundraiserNotificationCommand, FundraiserNotificationReq>($"Send{req.Type.Name}Email",
                                                                                             req);
    }
}
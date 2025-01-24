using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderCreatedHandler : CrowdfunderEventHandler<CrowdfunderCreatedEvent> {
    private readonly IContentService _contentService;
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingNotifications _crowdfundingNotifications;
    private readonly ICrowdfundingUrlBuilder _crowdfundingUrlBuilder;

    public CrowdfunderCreatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                     IContentService contentService,
                                     IContentLocator contentLocator,
                                     ICrowdfunderRevisionRepository crowdfunderRevisionRepository,
                                     ICrowdfundingNotifications crowdfundingNotifications,
                                     ICrowdfundingUrlBuilder crowdfundingUrlBuilder)
        : base(asyncKeyedLocker, contentService, contentLocator, crowdfunderRevisionRepository) {
        _contentService = contentService;
        _contentLocator = contentLocator;
        _crowdfundingNotifications = crowdfundingNotifications;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
    }

    protected override async Task HandleEventAsync(CrowdfunderCreatedEvent req,
                                                   IContent content,
                                                   CancellationToken cancellationToken) {
        var type = content.ContentType.Alias.ToCrowdfunderType();

        await AddOrUpdateRevisionAsync(content.Key, content.VersionId, type);

        UpdateStatus(content, type, req.Model.CrowdfunderInfo.Status.Name);

        if (type == CrowdfunderTypes.Fundraiser) {
            SendFundraiserCreatedEmail(content);
        }
    }

    private void UpdateStatus(IContent content, CrowdfunderType type, string statusName) {
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, statusName);

        if (type == CrowdfunderTypes.Campaign) {
            content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, true);
        } else {
            content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);
        }
    }

    private void SendFundraiserCreatedEmail(IContent content) {
        var fundraiser = _contentLocator.ById<FundraiserContent>(content.Key);
        var fundraiserContentViewModel = new FundraiserContentViewModel(_crowdfundingUrlBuilder, fundraiser);
        var model = new FundraiserNotificationViewModel(fundraiserContentViewModel, null);

        _crowdfundingNotifications.Enqueue(FundraiserNotificationTypes.FundraiserCreated, model, fundraiser.Key);
    }
}
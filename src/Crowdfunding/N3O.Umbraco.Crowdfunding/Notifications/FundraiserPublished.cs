using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class FundraiserPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly ICrowdfunderManager _crowdfunderManager;
    private readonly IContentLocator _contentLocator;

    public FundraiserPublished(ICrowdfunderManager crowdfunderManager, IContentLocator contentLocator) {
        _crowdfunderManager = crowdfunderManager;
        _contentLocator = contentLocator;
    }
    
    public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias)) {
                var fundraiser = _contentLocator.ById<FundraiserContent>(content.Key);
                
                if (!fundraiser.Status.HasValue()) {
                    await _crowdfunderManager.CreateFundraiserAsync(fundraiser);
                } else {
                    await _crowdfunderManager.UpdateCrowdfunderAsync(fundraiser.Key.ToString(),
                                                                     fundraiser,
                                                                     fundraiser.ToggleStatus);
                }
            }
        }
    }
}
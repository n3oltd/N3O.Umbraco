using Slugify;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class FundraiserSaving : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly ICrowdfundingHelper _crowdfundingHelper;
    private readonly ISlugHelper _slugHelper;

    public FundraiserSaving(ICrowdfundingHelper crowdfundingHelper, ISlugHelper slugHelper) {
        _crowdfundingHelper = crowdfundingHelper;
        _slugHelper = slugHelper;
    }
    
    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            if (_crowdfundingHelper.IsFundraiser(content)) {
                var title = content.GetValue<string>(CrowdfundingConstants.Fundraiser.Properties.Title);
                var slug = content.GetValue<string>(CrowdfundingConstants.Fundraiser.Properties.Slug);

                slug ??= _slugHelper.GenerateSlug(title);
                
                content.Name =  $"{title} ({slug})";
            }
        }

        return Task.CompletedTask;
    }
}
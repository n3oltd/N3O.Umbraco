using N3O.Umbraco.Extensions;
using Slugify;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

public class CrowdfundingPageSaving : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly IFundraisingPages _fundraisingPages;

    public CrowdfundingPageSaving(IFundraisingPages fundraisingPages) {
        _fundraisingPages = fundraisingPages;
    }
    
    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            if (_fundraisingPages.IsFundraisingPage(content)) {
                var pageTitle = content.GetValue<string>(CrowdfundingConstants.CrowdfundingPage.Properties.PageTitle);
                var slug = content.GetValue<string>(CrowdfundingConstants.CrowdfundingPage.Properties.PageSlug);

                if (!slug.HasValue()) {
                    var slugHelper = new SlugHelper();
                    
                    slug = slugHelper.GenerateSlug(pageTitle);
                }
                
                content.Name =  $"{pageTitle} ({slug})";
            }
        }

        return Task.CompletedTask;
    }
}
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using Slugify;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

[Order(1)]
public class FundraiserSaving : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly Lazy<ISlugHelper> _slugHelper;

    public FundraiserSaving(Lazy<ISlugHelper> slugHelper) {
        _slugHelper = slugHelper;
    }
    
    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            if (content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias)) {
                var name = content.GetValue<string>(CrowdfundingConstants.Crowdfunder.Properties.Name);
                var slug = content.GetValue<string>(CrowdfundingConstants.Fundraiser.Properties.Slug);

                slug ??= _slugHelper.Value.GenerateSlug(name);
                
                content.Name =  $"{name} ({slug})";
            }
        }

        return Task.CompletedTask;
    }
}
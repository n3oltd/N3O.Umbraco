using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Extensions;
using Slugify;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

[Order(1)]
[SkipDuringSync]
public class FundraiserSaving : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly Lazy<ISlugHelper> _slugHelper;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FundraiserSaving(Lazy<ISlugHelper> slugHelper, IWebHostEnvironment webHostEnvironment) {
        _slugHelper = slugHelper;
        _webHostEnvironment = webHostEnvironment;
    }
    
    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            if (content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias)) {
                var name = content.GetValue<string>(CrowdfundingConstants.Crowdfunder.Properties.Name);
                var slug = content.GetValue<string>(CrowdfundingConstants.Fundraiser.Properties.Slug);

                slug ??= _slugHelper.Value.GenerateSlug(name);
                
                content.Name = $"{name} ({slug})";
                
                if (!_webHostEnvironment.IsProduction()) {
                    content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, CrowdfunderStatuses.Active.Name);
                }
            }
        }

        return Task.CompletedTask;
    }
}
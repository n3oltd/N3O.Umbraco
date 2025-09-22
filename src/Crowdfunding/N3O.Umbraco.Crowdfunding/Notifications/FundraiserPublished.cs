using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Crowdfunding.Notifications;

[SkipDuringSync]
public class FundraiserPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IBackgroundJob _backgroundJob;

    public FundraiserPublished(IWebHostEnvironment webHostEnvironment,
                               IBackgroundJob backgroundJob) {
        _webHostEnvironment = webHostEnvironment;
        _backgroundJob = backgroundJob;
    }

    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        if (_webHostEnvironment.IsProduction()) {
            foreach (var content in notification.PublishedEntities) {
                if (content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraiser.Alias)) {
                    _backgroundJob.EnqueueCommand<FundraiserPublishedNotification>(p => {
                        p.Add<ContentId>(content.Key.ToString());
                    });
                }
            }
        }
        
        return Task.CompletedTask;
    }
}
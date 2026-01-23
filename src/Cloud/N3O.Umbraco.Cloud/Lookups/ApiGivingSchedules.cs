using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Lookups;

[Order(int.MaxValue)]
public class ApiGivingSchedules : ApiLookupsCollection<GivingSchedule> {
    private readonly ICdnClient _cdnClient;

    public ApiGivingSchedules(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<GivingSchedule>> FetchAsync(CancellationToken cancellationToken) {
        var publishedGivingSchedules = await _cdnClient.DownloadSubscriptionContentAsync<PublishedGivingSchedules>(SubscriptionFiles.GivingSchedules,
                                                                                                                   JsonSerializers.JsonProvider,
                                                                                                                   cancellationToken);

        var givingSchedules = new List<GivingSchedule>();

        foreach (var publishedGivingSchedule in publishedGivingSchedules.Schedules) {
            var givingSchedule = new GivingSchedule(publishedGivingSchedule.Id, publishedGivingSchedule.Name, null);
            
            givingSchedules.Add(givingSchedule);
        }

        return givingSchedules;
    }

    protected override TimeSpan CacheDuration => TimeSpan.FromMinutes(1);
}

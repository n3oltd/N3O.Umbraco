using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Lookups;

[Order(int.MaxValue)]
public class ApiQurbaniSeasons : ApiLookupsCollection<QurbaniSeason> {
    private readonly ICdnClient _cdnClient;

    public ApiQurbaniSeasons(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<QurbaniSeason>> FetchAsync(CancellationToken cancellationToken) {
        var publishedQurbaniSeasons = await _cdnClient.DownloadSubscriptionContentAsync<PublishedQurbaniSeasons>(SubscriptionFiles.QurbaniSeasons,
                                                                                                                 JsonSerializers.JsonProvider,
                                                                                                                 cancellationToken);

        var qurbaniSeasons = new List<QurbaniSeason>();

        foreach (var publishedQurbaniSeason in publishedQurbaniSeasons.OrEmpty(x => x.Seasons)) {
            var qurbaniSeason = new QurbaniSeason(publishedQurbaniSeason.Id, publishedQurbaniSeason.Name, null);
            
            qurbaniSeasons.Add(qurbaniSeason);
        }

        return qurbaniSeasons;
    }

    protected override TimeSpan ReloadInterval => TimeSpan.FromMinutes(1);
}

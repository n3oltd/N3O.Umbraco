using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Lookups;

[Order(int.MaxValue)]
public class ApiQurbaniItems : ApiLookupsCollection<QurbaniItem> {
    private readonly ICdnClient _cdnClient;

    public ApiQurbaniItems(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<QurbaniItem>> FetchAsync(CancellationToken cancellationToken) {
        var publishedQurbaniItems = await _cdnClient.DownloadSubscriptionContentAsync<PublishedQurbaniItems>(SubscriptionFiles.QurbaniItems,
                                                                                                             JsonSerializers.JsonProvider,
                                                                                                             cancellationToken);

        var qurbaniItems = new List<QurbaniItem>();

        foreach (var publishedQurbaniItem in publishedQurbaniItems.OrEmpty(x => x.Items)) {
            var qurbaniItem = new QurbaniItem(publishedQurbaniItem.Id, publishedQurbaniItem.Name, null);
            
            qurbaniItems.Add(qurbaniItem);
        }

        return qurbaniItems;
    }

    protected override TimeSpan ReloadInterval => TimeSpan.FromMinutes(1);
}

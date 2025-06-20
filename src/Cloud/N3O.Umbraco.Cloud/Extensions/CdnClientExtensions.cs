using N3O.Umbraco.Cloud.Lookups;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Extensions;

public static class CdnClientExtensions {
    public static async Task<T> DownloadSubscriptionContentAsync<T>(this ICdnClient cdnClient,
                                                                    SubscriptionFile file,
                                                                    CancellationToken cancellationToken = default) {
        var content = await cdnClient.DownloadPublishedContentAsync<T>(PublishedFileKinds.Subscription,
                                                                       file.Filename,
                                                                       cancellationToken);

        return content;
    }
}
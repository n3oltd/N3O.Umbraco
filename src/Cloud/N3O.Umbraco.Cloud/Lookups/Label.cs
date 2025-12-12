using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Lookups;

public class Label : NamedLookup {
    public Label(string id, string name) : base(id, name) { }
}

[Order(int.MaxValue)]
public abstract class Labels<T> : ApiLookupsCollection<T> where T : Label {
    private readonly ICdnClient _cdnClient;

    protected Labels(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<T>> FetchAsync(CancellationToken cancellationToken) {
        var publishedTagDefinitions = await _cdnClient.DownloadSubscriptionContentAsync<PublishedTagDefinitions>(SubscriptionFiles.TagDefinitions,
                                                                                                                 JsonSerializers.Simple,
                                                                                                                 cancellationToken);

        var labels = new List<T>();

        foreach (var publishedTagDefinition in publishedTagDefinitions.Definitions.OrEmpty().Where(x => x.Scope == Scope)) {
            var label = CreateLabel(publishedTagDefinition);
            
            labels.Add(label);
        }

        return labels;
    }

    protected abstract T CreateLabel(PublishedTagDefinition publishedTagDefinition);

    protected abstract TagScope Scope { get; }

    protected override TimeSpan CacheDuration => TimeSpan.FromHours(1);
}
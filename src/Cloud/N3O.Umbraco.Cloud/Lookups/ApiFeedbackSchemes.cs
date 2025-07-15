using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Lookups;

[Order(int.MaxValue)]
public class ApiFeedbackSchemes : ApiLookupsCollection<FeedbackScheme> {
    private readonly ICdnClient _cdnClient;

    public ApiFeedbackSchemes(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<FeedbackScheme>> FetchAsync(CancellationToken cancellationToken) {
        var publishedFeedbackSchemes = await _cdnClient.DownloadSubscriptionContentAsync<PublishedFeedbackSchemes>(SubscriptionFiles.FeedbackSchemes,
                                                                                                                   JsonSerializers.JsonProvider,
                                                                                                                   cancellationToken);

        var feedbackSchemes = new List<FeedbackScheme>();

        foreach (var publishedFeedbackScheme in publishedFeedbackSchemes.FeedbackSchemes) {
            var feedbackScheme = new FeedbackScheme(publishedFeedbackScheme.Id,
                                                    publishedFeedbackScheme.Name,
                                                    null,
                                                    publishedFeedbackScheme.AllowedGivingTypes,
                                                    publishedFeedbackScheme.FundDimensionOptions.IfNotNull(x => new FundDimensionOptions(x)),
                                                    publishedFeedbackScheme.Pricing.IfNotNull(x => new Pricing(x)),
                                                    publishedFeedbackScheme.CustomFieldDefinitions.OrEmpty().Select(x => new FeedbackCustomFieldDefinition(x)));
            
            feedbackSchemes.Add(feedbackScheme);
        }

        return feedbackSchemes;
    }

    protected override TimeSpan CacheDuration => TimeSpan.FromHours(12);
}

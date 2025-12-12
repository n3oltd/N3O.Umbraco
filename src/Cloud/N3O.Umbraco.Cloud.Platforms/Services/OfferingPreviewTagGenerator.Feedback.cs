/*using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Markup;
using N3O.Umbraco.Media;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using OfferingType = N3O.Umbraco.Cloud.Platforms.Lookups.OfferingType;
using PublishedGiftType = N3O.Umbraco.Cloud.Platforms.Clients.GiftType;

namespace N3O.Umbraco.Cloud.Platforms;

public class FeedbackOfferingPreviewTagGenerator : OfferingPreviewTagGenerator {
    public FeedbackOfferingPreviewTagGenerator(ICdnClient cdnClient,
                                                  IJsonProvider jsonProvider,
                                                  IMediaUrl mediaUrl,
                                                  ILookups lookups,
                                                  IUmbracoMapper mapper,
                                                  IMarkupEngine markupEngine,
                                                  IMediaLocator mediaLocator,
                                                  IPublishedValueFallback publishedValueFallback)
        : base(cdnClient,
               jsonProvider,
               mediaUrl,
               lookups,
               mapper,
               markupEngine,
               mediaLocator,
               publishedValueFallback) { }

    protected override OfferingType OfferingType => OfferingTypes.Feedback;
    
    protected override void PopulatePublishedOffering(IReadOnlyDictionary<string, object> content,
                                                      PublishedOffering publishedOffering) {
        var feedbackScheme = GetFeedbackScheme(content); 
        
        var publishedFeedbackOffering = new PublishedFeedbackOffering();
        publishedFeedbackOffering.Scheme = new PublishedOfferingFeedbackScheme();
        publishedFeedbackOffering.Scheme.Id = feedbackScheme.Id;
        publishedFeedbackOffering.Scheme.Name = feedbackScheme.Name;

        publishedFeedbackOffering.CustomFieldDefinitions = feedbackScheme
                                                             .CustomFields
                                                             .OrEmpty()
                                                             .Select(Mapper.Map<IFeedbackCustomFieldDefinition, PublishedFeedbackCustomFieldDefinition>)
                                                             .ToList();

        if (feedbackScheme.HasPricing()) {
            publishedFeedbackOffering.Pricing = feedbackScheme.Pricing.IfNotNull(Mapper.Map<IPricing, PublishedPricing>);
        }
        
        publishedOffering.Feedback = publishedFeedbackOffering;
    }
    
    protected override IFundDimensionOptions GetFundDimensionOptions(IReadOnlyDictionary<string, object> content) {
        var scheme = GetFeedbackScheme(content);
        
        return scheme?.FundDimensionOptions;
    }

    protected override IEnumerable<PublishedGiftType> GetPublishedSuggestedGiftTypes(IReadOnlyDictionary<string, object> content) {
        return GetFeedbackScheme(content)?.AllowedGivingTypes.Select(x => x.ToGiftType().ToEnum<PublishedGiftType>().GetValueOrThrow());
    }

    private FeedbackScheme GetFeedbackScheme(IReadOnlyDictionary<string, object> content) {
        return GetDataListValue<FeedbackScheme>(content, AliasHelper<FeedbackOfferingContent>.PropertyAlias(x => x.Scheme));
    }
}*/
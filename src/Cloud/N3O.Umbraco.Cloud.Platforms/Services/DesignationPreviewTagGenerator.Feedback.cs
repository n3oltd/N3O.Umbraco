using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Cloud.Platforms.Clients;
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
using DesignationType = N3O.Umbraco.Cloud.Platforms.Lookups.DesignationType;
using PublishedGiftType = N3O.Umbraco.Cloud.Platforms.Clients.GiftType;

namespace N3O.Umbraco.Cloud.Platforms;

public class FeedbackDesignationPreviewTagGenerator : DesignationPreviewTagGenerator {
    public FeedbackDesignationPreviewTagGenerator(ICdnClient cdnClient,
                                                  IJsonProvider jsonProvider,
                                                  IMediaUrl mediaUrl,
                                                  ILookups lookups,
                                                  IUmbracoMapper mapper,
                                                  IMarkupEngine markupEngine,
                                                  IMediaLocator mediaLocator,
                                                  IPublishedValueFallback publishedValueFallback,
                                                  IHtmlHelper htmlHelper) 
        : base(cdnClient, jsonProvider, mediaUrl, lookups, mapper, markupEngine, mediaLocator, publishedValueFallback, htmlHelper) { }
    
    protected override DesignationType DesignationType => DesignationTypes.Feedback;
    
    protected override void PopulatePublishedDesignation(IReadOnlyDictionary<string, object> content,
                                                         PublishedDesignation publishedDesignation) {
        var feedbackScheme = GetFeedbackScheme(content); 
        
        var publishedFeedbackDesignation = new PublishedFeedbackDesignation();
        publishedFeedbackDesignation.Scheme = new PublishedDesignationFeedbackScheme();
        publishedFeedbackDesignation.Scheme.Id = feedbackScheme.Id;
        publishedFeedbackDesignation.Scheme.Name = feedbackScheme.Name;

        publishedFeedbackDesignation.CustomFieldDefinitions = feedbackScheme
                                                             .CustomFields
                                                             .OrEmpty()
                                                             .Select(Mapper.Map<IFeedbackCustomFieldDefinition, PublishedFeedbackCustomFieldDefinition>)
                                                             .ToList();

        if (feedbackScheme.HasPricing()) {
            publishedFeedbackDesignation.Pricing = feedbackScheme.Pricing.IfNotNull(Mapper.Map<IPricing, PublishedPricing>);
        }
        
        publishedDesignation.Feedback = publishedFeedbackDesignation;
    }
    
    protected override IFundDimensionOptions GetFundDimensionOptions(IReadOnlyDictionary<string, object> content) {
        var scheme = GetFeedbackScheme(content);
        
        return scheme?.FundDimensionOptions;
    }

    protected override IEnumerable<PublishedGiftType> GetPublishedSuggestedGiftTypes(IReadOnlyDictionary<string, object> content) {
        return GetFeedbackScheme(content)?.AllowedGivingTypes.Select(x => x.ToGiftType().ToEnum<PublishedGiftType>().GetValueOrThrow());
    }

    private FeedbackScheme GetFeedbackScheme(IReadOnlyDictionary<string, object> content) {
        return GetDataListValue<FeedbackScheme>(content, AliasHelper<FeedbackDesignationContent>.PropertyAlias(x => x.Scheme));
    }
}
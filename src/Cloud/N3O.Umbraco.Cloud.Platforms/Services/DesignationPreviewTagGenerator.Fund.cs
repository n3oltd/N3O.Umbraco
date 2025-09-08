using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
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

public class FundDesignationPreviewTagGenerator : DesignationPreviewTagGenerator {
    private readonly IJsonProvider _jsonProvider;

    public FundDesignationPreviewTagGenerator(ICdnClient cdnClient,
                                              IJsonProvider jsonProvider,
                                              IMediaUrl mediaUrl,
                                              ILookups lookups,
                                              IUmbracoMapper mapper,
                                              IMarkupEngine markupEngine,
                                              IMediaLocator mediaLocator,
                                              IPublishedValueFallback publishedValueFallback,
                                              IHtmlHelper htmlHelper) 
        : base(cdnClient, jsonProvider, mediaUrl, lookups, mapper, markupEngine, mediaLocator, publishedValueFallback, htmlHelper) {
        _jsonProvider = jsonProvider;
    }
    
    protected override DesignationType DesignationType => DesignationTypes.Fund;
    
    protected override void PopulatePublishedDesignation(IReadOnlyDictionary<string, object> content,
                                                         PublishedDesignation publishedDesignation) {
        var donationItem =  GetDonationItem(content);
        
        var oneTimeSuggestedAmounts = GetSuggestedAmounts(content, AliasHelper<FundDesignationContent>.PropertyAlias(x => x.OneTimeSuggestedAmounts));
        var recurringSuggestedAmounts = GetSuggestedAmounts(content, AliasHelper<FundDesignationContent>.PropertyAlias(x => x.RecurringSuggestedAmounts));
        
        var publishedFundDesignation = new PublishedFundDesignation();
        publishedFundDesignation.Item = new PublishedDesignationDonationItem();
        publishedFundDesignation.Item.Id = donationItem?.Id;
        publishedFundDesignation.Item.Name = donationItem?.Name;
        publishedFundDesignation.Item.Pricing = donationItem?.Pricing.IfNotNull(Mapper.Map<IPricing, PublishedPricing>);
        
        publishedFundDesignation.SuggestedAmounts = new PublishedSuggestedAmounts();
        publishedFundDesignation.SuggestedAmounts.OneTime = oneTimeSuggestedAmounts.ToList();
        publishedFundDesignation.SuggestedAmounts.Recurring = recurringSuggestedAmounts.ToList();
        
        publishedDesignation.Fund = publishedFundDesignation;
    }

    protected override IFundDimensionOptions GetFundDimensionOptions(IReadOnlyDictionary<string, object> content) {
        var donationItem = GetDonationItem(content);
        
        return donationItem?.FundDimensionOptions;
    }
    
    protected override IEnumerable<PublishedGiftType> GetPublishedSuggestedGiftTypes(IReadOnlyDictionary<string, object> content) {
        return GetDonationItem(content)?.AllowedGivingTypes.Select(x => x.ToGiftType().ToEnum<PublishedGiftType>().GetValueOrThrow());
    }
    
    private DonationItem GetDonationItem(IReadOnlyDictionary<string, object> content) {
        return GetDataListValue<DonationItem>(content, AliasHelper<FundDesignationContent>.PropertyAlias(x => x.DonationItem));
    }
    
    private IReadOnlyList<PublishedSuggestedAmount> GetSuggestedAmounts(IReadOnlyDictionary<string, object> content, string alias) {
        var suggestedAmounts = _jsonProvider.DeserializeObject<IEnumerable<PublishedSuggestedAmount>>(content[alias]?.ToString()).ToList();
       
        return suggestedAmounts.OrEmpty().ToList();
    }
}
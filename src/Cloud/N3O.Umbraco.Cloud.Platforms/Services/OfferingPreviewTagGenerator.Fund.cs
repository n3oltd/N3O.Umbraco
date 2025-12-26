/*using N3O.Umbraco.Cloud.Clients.Platforms;
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
using OfferingType = N3O.Umbraco.Cloud.Platforms.Lookups.OfferingType;
using PublishedGiftType = N3O.Umbraco.Cloud.Clients.Platforms.GiftType;

namespace N3O.Umbraco.Cloud.Platforms;

public class FundOfferingPreviewTagGenerator : OfferingPreviewTagGenerator {
    private readonly IJsonProvider _jsonProvider;

    public FundOfferingPreviewTagGenerator(ICdnClient cdnClient,
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
               publishedValueFallback) {
        _jsonProvider = jsonProvider;
    }
    
    protected override OfferingType OfferingType => OfferingTypes.Fund;
    
    protected override void PopulatePublishedOffering(IReadOnlyDictionary<string, object> content,
                                                      PublishedOffering publishedOffering) {
        var donationItem = GetDonationItem(content);
        
        var oneTimeSuggestedAmounts = GetSuggestedAmounts(content, AliasHelper<FundOfferingContent>.PropertyAlias(x => x.OneTimeSuggestedAmounts));
        var recurringSuggestedAmounts = GetSuggestedAmounts(content, AliasHelper<FundOfferingContent>.PropertyAlias(x => x.RecurringSuggestedAmounts));
        
        var publishedFundOffering = new PublishedFundOffering();
        publishedFundOffering.Item = new PublishedOfferingDonationItem();
        publishedFundOffering.Item.Id = donationItem?.Id;
        publishedFundOffering.Item.Name = donationItem?.Name;
        publishedFundOffering.Item.Pricing = donationItem?.Pricing.IfNotNull(Mapper.Map<IPricing, PublishedPricing>);
        
        publishedFundOffering.SuggestedAmounts = new PublishedSuggestedAmounts();
        publishedFundOffering.SuggestedAmounts.OneTime = oneTimeSuggestedAmounts.OrEmpty().ToList();
        publishedFundOffering.SuggestedAmounts.Recurring = recurringSuggestedAmounts.OrEmpty().ToList();
        
        publishedOffering.Fund = publishedFundOffering;
    }

    protected override IFundDimensionOptions GetFundDimensionOptions(IReadOnlyDictionary<string, object> content) {
        var donationItem = GetDonationItem(content);
        
        return donationItem?.FundDimensionOptions;
    }
    
    protected override IEnumerable<PublishedGiftType> GetPublishedSuggestedGiftTypes(IReadOnlyDictionary<string, object> content) {
        return GetDonationItem(content)?.AllowedGivingTypes.Select(x => x.ToGiftType().ToEnum<PublishedGiftType>().GetValueOrThrow());
    }
    
    private DonationItem GetDonationItem(IReadOnlyDictionary<string, object> content) {
        return GetDataListValue<DonationItem>(content, AliasHelper<FundOfferingContent>.PropertyAlias(x => x.DonationItem));
    }
    
    private IReadOnlyList<PublishedSuggestedAmount> GetSuggestedAmounts(IReadOnlyDictionary<string, object> content, string alias) {
        var suggestedAmountsStr = content[alias]?.ToString();

        if (suggestedAmountsStr.HasValue()) {
            return _jsonProvider.DeserializeObject<IEnumerable<PublishedSuggestedAmount>>(suggestedAmountsStr).ToList();
        } else {
            return null;
        }
    }
}*/
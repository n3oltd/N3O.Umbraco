using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
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
using Umbraco.Cms.Core.Models.PublishedContent;
using AllocationType = N3O.Umbraco.Cloud.Platforms.Clients.AllocationType;
using OfferingType = N3O.Umbraco.Cloud.Platforms.Lookups.OfferingType;
using PublishedDonationItem = N3O.Umbraco.Cloud.Platforms.Clients.PublishedDonationItem;
using PublishedFundDimensionOptions = N3O.Umbraco.Cloud.Platforms.Clients.PublishedFundDimensionOptions;
using PublishedFundDimensionValues = N3O.Umbraco.Cloud.Platforms.Clients.PublishedFundDimensionValues;
using PublishedGiftType = N3O.Umbraco.Cloud.Platforms.Clients.GiftType;
using PublishedPrice = N3O.Umbraco.Cloud.Platforms.Clients.PublishedPrice;
using PublishedPricing = N3O.Umbraco.Cloud.Platforms.Clients.PublishedPricing;
using PublishedPricingRule = N3O.Umbraco.Cloud.Platforms.Clients.PublishedPricingRule;

namespace N3O.Umbraco.Cloud.Platforms;

public class FundOfferingPreviewTagGenerator : OfferingPreviewTagGenerator {
    private readonly IJsonProvider _jsonProvider;
    private readonly ILookups _lookups;

    public FundOfferingPreviewTagGenerator(ICdnClient cdnClient,
                                           IJsonProvider jsonProvider,
                                           IMediaUrl mediaUrl,
                                           ILookups lookups,
                                           IMarkupEngine markupEngine,
                                           IMediaLocator mediaLocator,
                                           IPublishedValueFallback publishedValueFallback,
                                           IBaseCurrencyAccessor baseCurrencyAccessor)
        : base(cdnClient,
               jsonProvider,
               mediaUrl,
               lookups,
               markupEngine,
               mediaLocator,
               publishedValueFallback,
               baseCurrencyAccessor) {
        _jsonProvider = jsonProvider;
        _lookups = lookups;
    }
    
    protected override OfferingType OfferingType => OfferingTypes.Fund;

    protected override void PopulateAllocationIntent(IReadOnlyDictionary<string, object> content,
                                                     PublishedAllocationIntent allocationIntent) {
        var donationItem = GetDonationItem(content);

        allocationIntent.Type = AllocationType.Fund;
        allocationIntent.Fund = new PublishedFundIntent();
        allocationIntent.Fund.DonationItem = donationItem.Id;
    }

    protected override void PopulateFormStateOptions(IReadOnlyDictionary<string, object> content,
                                                     PublishedDonationFormOptions options) {
        var oneTimeSuggestedAmounts = GetDonationFormSuggestedAmountsReq(content, AliasHelper<FundOfferingContent>.PropertyAlias(x => x.OneTimeSuggestedAmounts), PublishedGiftType.OneTime);
        var recurringSuggestedAmounts = GetDonationFormSuggestedAmountsReq(content, AliasHelper<FundOfferingContent>.PropertyAlias(x => x.RecurringSuggestedAmounts), PublishedGiftType.Recurring);
        
        var suggestedAmounts = new List<DonationFormSuggestedAmountsReq>();
        suggestedAmounts.Add(oneTimeSuggestedAmounts);
        suggestedAmounts.Add(recurringSuggestedAmounts);
        
        options.SuggestedAmounts = suggestedAmounts;
    }

    protected override void PopulateAdditionalData(Dictionary<string, object> previewData, PublishedDonationForm publishedDonationForm) {
        var donationItem = _lookups.FindById<DonationItem>(publishedDonationForm.FormState.CartItem.NewDonation.Allocation.Fund.DonationItem);
        
        var fundDimensionOptions = new PublishedFundDimensionOptions();
        fundDimensionOptions.Dimension1 = donationItem.FundDimensionOptions.Dimension1?.Select(x => x.Name).ToList();
        fundDimensionOptions.Dimension2 = donationItem.FundDimensionOptions.Dimension2?.Select(x => x.Name).ToList();
        fundDimensionOptions.Dimension3 = donationItem.FundDimensionOptions.Dimension3?.Select(x => x.Name).ToList();
        fundDimensionOptions.Dimension4 = donationItem.FundDimensionOptions.Dimension4?.Select(x => x.Name).ToList();
        
        var publishedDonationItem = new PublishedDonationItem();
        publishedDonationItem.Id = donationItem.Id;
        publishedDonationItem.Name = donationItem.Name;
        publishedDonationItem.AllowOneTime = donationItem.AllowedGivingTypes.HasAny(x => x == GivingTypes.Donation);
        publishedDonationItem.AllowOneTime = donationItem.AllowedGivingTypes.HasAny(x => x == GivingTypes.RegularGiving);
        publishedDonationItem.FundDimensionOptions = fundDimensionOptions;
        
        if (donationItem.HasPricing()) {
            publishedDonationItem.Pricing = GetPricing(donationItem);
        }
        
        previewData["donationItem"] = publishedDonationItem;
    }

    protected override IFundDimensionOptions GetFundDimensionOptions(IReadOnlyDictionary<string, object> content) {
        var donationItem = GetDonationItem(content);
        
        return donationItem?.FundDimensionOptions;
    }
    
    private DonationItem GetDonationItem(IReadOnlyDictionary<string, object> content) {
        return GetDataListValue<DonationItem>(content, AliasHelper<FundOfferingContent>.PropertyAlias(x => x.DonationItem));
    }
    
    private DonationFormSuggestedAmountsReq GetDonationFormSuggestedAmountsReq(IReadOnlyDictionary<string, object> content,
                                                                               string alias,
                                                                               PublishedGiftType giftType) {
        var suggestedAmountsStr = content[alias]?.ToString();

        if (suggestedAmountsStr.HasValue()) {
            var suggested = new DonationFormSuggestedAmountsReq();
            suggested.GiftType = giftType;
            suggested.Amounts = _jsonProvider.DeserializeObject<IEnumerable<DonationFormSuggestedAmountReq>>(suggestedAmountsStr).ToList();

            return suggested;
        } else {
            return null;
        }
    }
    
    private PublishedPricing GetPricing(DonationItem donationItem) {
        var publishedPriceRules = new List<PublishedPricingRule>();
        
        foreach (var pricingRule in donationItem.Pricing.Rules.OrEmpty()) {
            var publishedRule = new PublishedPricingRule();
            
            publishedRule.Price = new PublishedPrice();
            publishedRule.Price.Amount = (double) pricingRule.Price.Amount;
            publishedRule.Price.Locked = pricingRule.Price.Locked;

            publishedRule.FundDimensions = new PublishedFundDimensionValues();
            publishedRule.FundDimensions.Dimension1 = pricingRule.FundDimensions.Dimension1?.Name;
            publishedRule.FundDimensions.Dimension2 = pricingRule.FundDimensions.Dimension2?.Name;
            publishedRule.FundDimensions.Dimension3 = pricingRule.FundDimensions.Dimension3?.Name;
            publishedRule.FundDimensions.Dimension4 = pricingRule.FundDimensions.Dimension4?.Name;
        }
        
        var publishedPricing = new PublishedPricing();
        publishedPricing.Price = new PublishedPrice();
        publishedPricing.Price.Amount = (double) donationItem.Pricing.Price.Amount;
        publishedPricing.Price.Locked = donationItem.Pricing.Price.Locked;
        publishedPricing.Rules = publishedPriceRules;
        
        return publishedPricing;
    }
}
/*using N3O.Umbraco.Cloud.Platforms.Clients;
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
using PublishedFeedbackCustomFieldDefinition = N3O.Umbraco.Cloud.Platforms.Clients.PublishedFeedbackCustomFieldDefinition;
using PublishedFeedbackCustomFieldTextFieldOptions = N3O.Umbraco.Cloud.Platforms.Clients.PublishedFeedbackCustomFieldTextFieldOptions;
using PublishedFeedbackCustomFieldType = N3O.Umbraco.Cloud.Platforms.Clients.PublishedFeedbackCustomFieldType;
using PublishedFeedbackScheme = N3O.Umbraco.Cloud.Platforms.Clients.PublishedFeedbackScheme;
using PublishedFundDimensionOptions = N3O.Umbraco.Cloud.Platforms.Clients.PublishedFundDimensionOptions;
using PublishedFundDimensionValues = N3O.Umbraco.Cloud.Platforms.Clients.PublishedFundDimensionValues;
using PublishedPrice = N3O.Umbraco.Cloud.Platforms.Clients.PublishedPrice;
using PublishedPricing = N3O.Umbraco.Cloud.Platforms.Clients.PublishedPricing;
using PublishedPricingRule = N3O.Umbraco.Cloud.Platforms.Clients.PublishedPricingRule;

namespace N3O.Umbraco.Cloud.Platforms;

public class FeedbackOfferingPreviewTagGenerator : OfferingPreviewTagGenerator {
    private readonly ILookups _lookups;

    public FeedbackOfferingPreviewTagGenerator(ICdnClient cdnClient,
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
        _lookups = lookups;
    }

    protected override OfferingType OfferingType => OfferingTypes.Feedback;

    protected override void PopulateAllocationIntent(IReadOnlyDictionary<string, object> content,
                                                     PublishedAllocationIntent allocationIntent) {
        var feedbackScheme = GetFeedbackScheme(content); 
        
        allocationIntent.Type = AllocationType.Feedback;
        allocationIntent.Feedback = new PublishedFeedbackIntent();
        allocationIntent.Feedback.Scheme = feedbackScheme.Id;
    }

    protected override void PopulateAdditionalData(Dictionary<string, object> previewData, PublishedDonationForm publishedDonationForm) {
        var feedbackScheme = _lookups.FindById<FeedbackScheme>(publishedDonationForm.FormState.CartItem.NewDonation.Allocation.Feedback.Scheme);
        
        var fundDimensionOptions = new PublishedFundDimensionOptions();
        fundDimensionOptions.Dimension1 = feedbackScheme.FundDimensionOptions.Dimension1?.Select(x => x.Name).ToList();
        fundDimensionOptions.Dimension2 = feedbackScheme.FundDimensionOptions.Dimension2?.Select(x => x.Name).ToList();
        fundDimensionOptions.Dimension3 = feedbackScheme.FundDimensionOptions.Dimension3?.Select(x => x.Name).ToList();
        fundDimensionOptions.Dimension4 = feedbackScheme.FundDimensionOptions.Dimension4?.Select(x => x.Name).ToList();
        
        var publishedFeedbackScheme = new PublishedFeedbackScheme();
        publishedFeedbackScheme.Id = feedbackScheme.Id;
        publishedFeedbackScheme.Name = feedbackScheme.Name;
        publishedFeedbackScheme.FundDimensionOptions = fundDimensionOptions;
        publishedFeedbackScheme.CustomFieldDefinitions = GetCustomFieldDefinitions(feedbackScheme).ToList();

        if (feedbackScheme.HasPricing()) {
            publishedFeedbackScheme.Pricing = GetPricing(feedbackScheme);
        }
        
        previewData["feedbackScheme"] = publishedFeedbackScheme;
    }

    private PublishedPricing GetPricing(FeedbackScheme feedbackScheme) {
        var publishedPriceRules = new List<PublishedPricingRule>();
        
        foreach (var pricingRule in feedbackScheme.Pricing.Rules.OrEmpty()) {
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
        publishedPricing.Price.Amount = (double) feedbackScheme.Pricing.Price.Amount;
        publishedPricing.Price.Locked = feedbackScheme.Pricing.Price.Locked;
        publishedPricing.Rules = publishedPriceRules;
        
        return publishedPricing;
    }

    private IEnumerable<PublishedFeedbackCustomFieldDefinition> GetCustomFieldDefinitions(FeedbackScheme feedbackScheme) {
        var items = new List<PublishedFeedbackCustomFieldDefinition>();
        
        foreach (var customFieldDefinition in feedbackScheme.CustomFields.OrEmpty()) {
            var publishedCustomField = new PublishedFeedbackCustomFieldDefinition();
            publishedCustomField.Alias = customFieldDefinition.Alias;
            publishedCustomField.Name = customFieldDefinition.Name;
            publishedCustomField.Required = customFieldDefinition.Required;

            publishedCustomField.Type = new PublishedFeedbackCustomFieldType();
            publishedCustomField.Type.Id = customFieldDefinition.Type.Id;
            publishedCustomField.Type.Name = customFieldDefinition.Type.Name;

            if (customFieldDefinition.Type == FeedbackCustomFieldTypes.Text) {
                publishedCustomField.Text = new PublishedFeedbackCustomFieldTextFieldOptions();
                publishedCustomField.Text.MaxLength = customFieldDefinition.TextMaxLength;
            }
            
            items.Add(publishedCustomField);
        }

        return items;
    }

    protected override IFundDimensionOptions GetFundDimensionOptions(IReadOnlyDictionary<string, object> content) {
        var scheme = GetFeedbackScheme(content);
        
        return scheme?.FundDimensionOptions;
    }

    private FeedbackScheme GetFeedbackScheme(IReadOnlyDictionary<string, object> content) {
        return GetDataListValue<FeedbackScheme>(content, AliasHelper<FeedbackOfferingContent>.PropertyAlias(x => x.Scheme));
    }
}*/
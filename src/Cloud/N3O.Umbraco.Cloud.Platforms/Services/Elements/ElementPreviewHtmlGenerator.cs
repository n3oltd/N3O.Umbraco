using Humanizer;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Extensions;
using AllocationType = N3O.Umbraco.Cloud.Platforms.Clients.AllocationType;
using ElementType = N3O.Umbraco.Cloud.Platforms.Lookups.ElementType;
using PlatformsPublishedPrice = N3O.Umbraco.Cloud.Platforms.Clients.PublishedPrice;
using PlatformsPublishedSponsorshipScheme = N3O.Umbraco.Cloud.Platforms.Clients.PublishedSponsorshipScheme;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class ElementPreviewHtmlGenerator : PreviewHtmlGenerator {
    private readonly IContentLocator _contentLocator;
    private readonly IUmbracoMapper _mapper;
    private readonly IBaseCurrencyAccessor _baseCurrencyAccessor;
    
    protected ElementPreviewHtmlGenerator(ICdnClient cdnClient,
                                         IJsonProvider jsonProvider,
                                         IContentLocator contentLocator,
                                         IUmbracoMapper mapper,
                                         ILookups lookups,
                                         IBaseCurrencyAccessor baseCurrencyAccessor)
        : base(cdnClient, jsonProvider, lookups) {
        _contentLocator = contentLocator;
        _mapper = mapper;
        _baseCurrencyAccessor = baseCurrencyAccessor;
    }
    
    protected abstract ElementType ElementType { get; }

    protected override string ContentTypeAlias => ElementType.ContentTypeAlias;

    protected override void PopulatePreviewData(IReadOnlyDictionary<string, object> content,
                                                Dictionary<string, object> previewData) {
        var campaignUdi = content[AliasHelper<ElementContent>.PropertyAlias(x => x.Campaign)]?.ToString();
        var offeringUdi = content[AliasHelper<DonationElementContent<DonationFormElementContent>>.PropertyAlias(x => x.Offering)]?.ToString();

        OfferingContent offeringContent;
        
        if (campaignUdi.HasValue()) {
            var campaign = _contentLocator.ById<CampaignContent>(UdiParser.Parse(campaignUdi).ToId().Value);
            offeringContent = campaign.DefaultOffering;}
        else if (offeringUdi.HasValue()) {
            offeringContent = _contentLocator.ById<OfferingContent>(UdiParser.Parse(offeringUdi).ToId().Value);
        }
        else {
            var defaultCampaign = _contentLocator.Single<PlatformsContent>().Campaigns.First();

            offeringContent = defaultCampaign.DefaultOffering;
        }
        
        var publishedOffering = _mapper.Map<OfferingContent, PublishedOffering>(offeringContent);
        
        var publishedDonationForm = new PublishedDonationForm();
        publishedDonationForm.Id = content[AliasHelper<OfferingContent>.PropertyAlias(x => x.Key)].ToString();
        publishedDonationForm.FormState = GetPublishedDonationFormState(content, publishedOffering);
        
        previewData["element"] = publishedDonationForm;
        previewData["offering"] = publishedOffering;

        PopulateAdditionalData(previewData, content, offeringContent);
    }

    private void PopulateAdditionalData(Dictionary<string, object> previewData,
                                        IReadOnlyDictionary<string, object> content,
                                        OfferingContent offeringContent) {
        if (offeringContent.Type == OfferingTypes.Fund) {
            var donationItem = offeringContent.Fund.DonationItem;
            
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
        } else if (offeringContent.Type == OfferingTypes.Feedback) {
            var feedbackScheme = offeringContent.Feedback.Scheme;
        
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
        } else if (offeringContent.Type == OfferingTypes.Sponsorship) {
            var sponsorshipScheme =offeringContent.Sponsorship.Scheme;
        
            previewData["beneficiaries"] = GetBeneficiaries(sponsorshipScheme);
            previewData["scheme"] = GetSponsorshipSchemes(sponsorshipScheme);
        }
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

    private PublishedDonationFormState GetPublishedDonationFormState(IReadOnlyDictionary<string, object> content,
                                                                     PublishedOffering publishedOffering) {
        var publishedOfferingAllocation = publishedOffering.FormState.CartItem.NewRegularGiving?.Allocation ?? publishedOffering.FormState.CartItem.NewDonation.Allocation;
        var currency = _baseCurrencyAccessor.GetBaseCurrency().ToEnum<Currency>();
        
        var formState = new PublishedDonationFormState();
        formState.CartItem = new PublishedCartItem();
        formState.CartItem.Type = CartItemType.NewDonation;
        formState.CartItem.Currency = currency;
        formState.CartItem.Value = new MoneyRes();
        formState.CartItem.Value.Currency = currency;
        formState.CartItem.Value.Amount = 0d;
        
        formState.CartItem.Type = CartItemType.NewDonation;
        
        formState.CartItem.NewDonation = new PublishedNewDonation();
        formState.CartItem.NewDonation.Allocation = new PublishedAllocationIntent();
        formState.CartItem.NewDonation.Allocation.FundDimensions = GetPublishedOfferingFundDimensions(content, publishedOfferingAllocation);
        formState.CartItem.NewDonation.Allocation.Value = new Money();
        formState.CartItem.NewDonation.Allocation.Value.Currency = currency;
        formState.CartItem.NewDonation.Allocation.Value.Amount = 0d;

        formState.CartItem.NewDonation.Allocation.Type = publishedOfferingAllocation.Type;

        if (publishedOfferingAllocation.Type == AllocationType.Fund) {
            formState.CartItem.NewDonation.Allocation.Fund = new PublishedFundIntent();
            formState.CartItem.NewDonation.Allocation.Fund.DonationItem = publishedOfferingAllocation.Fund.DonationItem;
        } else if (publishedOfferingAllocation.Type == AllocationType.Feedback) {
            formState.CartItem.NewDonation.Allocation.Feedback = new PublishedFeedbackIntent();
            formState.CartItem.NewDonation.Allocation.Feedback.Scheme = publishedOfferingAllocation.Feedback.Scheme;
        } else if (publishedOfferingAllocation.Type == AllocationType.Sponsorship) {
            formState.CartItem.NewDonation.Allocation.Sponsorship = new PublishedSponsorshipIntent();
            formState.CartItem.NewDonation.Allocation.Sponsorship.Scheme = publishedOfferingAllocation.Sponsorship.Scheme;
        }
        
        if (publishedOfferingAllocation.Type == AllocationType.Fund) {
            publishedOffering.FormState.Options = new PublishedDonationFormOptions();
            publishedOffering.FormState.Options.SuggestedAmounts = publishedOffering.FormState.Options.SuggestedAmounts;
        }

        return formState;
    }
    
    private PublishedPricing GetPricing(IHoldPricing pricing) {
        var publishedPriceRules = new List<PublishedPricingRule>();
        
        foreach (var pricingRule in pricing.Pricing.Rules.OrEmpty()) {
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
        publishedPricing.Price.Amount = (double) pricing.Pricing.Price.Amount;
        publishedPricing.Price.Locked = pricing.Pricing.Price.Locked;
        publishedPricing.Rules = publishedPriceRules;
        
        return publishedPricing;
    }
    
    private PublishedFundDimensionValues GetPublishedOfferingFundDimensions(IReadOnlyDictionary<string, object> content, PublishedAllocationIntent publishedOfferingAllocation) {
        var dimension1 = GetDataListValue<FundDimension1Value>(content, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension1));
        var dimension2 = GetDataListValue<FundDimension2Value>(content, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension2));
        var dimension3 = GetDataListValue<FundDimension3Value>(content, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension3));
        var dimension4 = GetDataListValue<FundDimension4Value>(content, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension4));
        
        var publishedFundDimensions = new PublishedFundDimensionValues();
        publishedFundDimensions.Dimension1 = dimension1?.Name ?? publishedOfferingAllocation.FundDimensions.Dimension1;
        publishedFundDimensions.Dimension2 = dimension2?.Name ?? publishedOfferingAllocation.FundDimensions.Dimension2;
        publishedFundDimensions.Dimension3 = dimension3?.Name ?? publishedOfferingAllocation.FundDimensions.Dimension3;
        publishedFundDimensions.Dimension4 = dimension4?.Name ?? publishedOfferingAllocation.FundDimensions.Dimension4;

        return publishedFundDimensions;
    }
    
    private object GetSponsorshipSchemes(SponsorshipScheme sponsorshipScheme) {
        var fundDimensionOptions = new PublishedSponsorshipSchemeFundDimensionOptions();
        fundDimensionOptions.Dimension1 = sponsorshipScheme.FundDimensionOptions.Dimension1?.Select(x => x.Name).ToList();
        fundDimensionOptions.Dimension2 = sponsorshipScheme.FundDimensionOptions.Dimension2?.Select(x => x.Name).ToList();
        fundDimensionOptions.Dimension3 = sponsorshipScheme.FundDimensionOptions.Dimension3?.Select(x => x.Name).ToList();
        fundDimensionOptions.Dimension4 = sponsorshipScheme.FundDimensionOptions.Dimension4?.Select(x => x.Name).ToList();
        
        var publishedSponsorshipScheme = new PlatformsPublishedSponsorshipScheme();
        publishedSponsorshipScheme.Id = sponsorshipScheme.Id;
        publishedSponsorshipScheme.Name = sponsorshipScheme.Name;
        publishedSponsorshipScheme.AvailableLocations = sponsorshipScheme.AvailableLocations.ToList();
        publishedSponsorshipScheme.FundDimensionOptions = fundDimensionOptions;
        
        
        return publishedSponsorshipScheme;
    }
    
    private IEnumerable<JObject> GetBeneficiaries(SponsorshipScheme sponsorshipScheme) {
        var name = "John Doe";
        
        var publishedBeneficiary = new PublishedBeneficiary();
        publishedBeneficiary.Id = name.Camelize();
        publishedBeneficiary.Type = BeneficiaryType.Child;
        publishedBeneficiary.Name = name;
        publishedBeneficiary.Location = "Pakistan";
        publishedBeneficiary.AvailableComponents = GetComponents(sponsorshipScheme).ToList();
        
        publishedBeneficiary.Individual = new PublishedIndividualBeneficiary();
        publishedBeneficiary.Individual.Age = 10;
        publishedBeneficiary.Individual.FirstName = "John";
        publishedBeneficiary.Individual.LastName = "Doe";
        publishedBeneficiary.Individual.Gender = Gender.Male;

        var view = new PublishedBeneficiaryPlatformsThemeViews();
        view.ThemeAlias = "default";
        view.DonationFormCaption = $"<p>{name} enjoys reading and hopes to become a doctor one day.</p>";
        view.DonationFormProfile = $"<p>{name} enjoys reading and hopes to become a doctor one day.</p>";
        
        publishedBeneficiary.PlatformsViews = new PublishedBeneficiaryPlatformsViews();
        publishedBeneficiary.PlatformsViews.Themes = view.Yield().ToList();

        var publishedBeneficiaryJObject = JObject.Parse(JsonConvert.SerializeObject(publishedBeneficiary));

        publishedBeneficiaryJObject["fundDimensions"] = new JObject();
        publishedBeneficiaryJObject["fundDimensions"]["dimension1"] = "Pakistan";
        publishedBeneficiaryJObject["fundDimensions"]["dimension2"] = "General";
        
        return publishedBeneficiaryJObject.Yield();
    }

    private IEnumerable<PublishedBeneficiaryComponent> GetComponents(SponsorshipScheme sponsorshipScheme) {
        var component = new PublishedBeneficiaryComponent();
        component.Name = sponsorshipScheme.Components.First().Name;
        component.Quantity = 1;
        component.Price = new PlatformsPublishedPrice();
        component.Price.Amount = (double) sponsorshipScheme.Components.First().Pricing.Price.Amount;
        component.Price.Locked = sponsorshipScheme.Components.First().Pricing.Price.Locked;
        
        return component.Yield();
    }
}
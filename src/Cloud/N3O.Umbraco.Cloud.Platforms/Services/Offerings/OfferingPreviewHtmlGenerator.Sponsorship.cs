using Humanizer;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Markup;
using N3O.Umbraco.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;
using AllocationType = N3O.Umbraco.Cloud.Platforms.Clients.AllocationType;
using OfferingType = N3O.Umbraco.Cloud.Platforms.Lookups.OfferingType;
using PlatformsPublishedSponsorshipScheme = N3O.Umbraco.Cloud.Platforms.Clients.PublishedSponsorshipScheme;
using PlatformsPublishedPrice = N3O.Umbraco.Cloud.Platforms.Clients.PublishedPrice;

namespace N3O.Umbraco.Cloud.Platforms;

public class SponsorshipOfferingPreviewHtmlGenerator : OfferingPreviewHtmlGenerator {
    private readonly ILookups _lookups;

    public SponsorshipOfferingPreviewHtmlGenerator(ICdnClient cdnClient,
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
    
    protected override OfferingType OfferingType => OfferingTypes.Sponsorship;

    protected override void PopulateAllocationIntent(IReadOnlyDictionary<string, object> content, PublishedAllocationIntent allocationIntent) {
        var scheme = GetSponsorshipScheme(content);
        
        allocationIntent.Type = AllocationType.Sponsorship;
        allocationIntent.Sponsorship = new PublishedSponsorshipIntent();
        allocationIntent.Sponsorship.Scheme = scheme.Id;
    }
    
    protected override IFundDimensionOptions GetFundDimensionOptions(IReadOnlyDictionary<string, object> content) {
        var scheme = GetSponsorshipScheme(content);
        
        return scheme?.FundDimensionOptions;
    }
    
    private SponsorshipScheme GetSponsorshipScheme(IReadOnlyDictionary<string, object> content) {
        return GetDataListValue<SponsorshipScheme>(content, AliasHelper<SponsorshipOfferingContent>.PropertyAlias(x => x.Scheme));
    }

    protected override void PopulateAdditionalData(Dictionary<string, object> previewData,
                                                   PublishedDonationForm publishedDonationForm) {
        var sponsorshipScheme = _lookups.FindById<SponsorshipScheme>(publishedDonationForm.FormState.CartItem.NewDonation.Allocation.Sponsorship.Scheme);
        
        previewData["beneficiaries"] = GetBeneficiaries(sponsorshipScheme);
        previewData["scheme"] = GetSponsorshipSchemes(sponsorshipScheme);
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
        
        publishedBeneficiary.PlatformsViews = new PublishedBeneficiaryPlatformsViews();
        publishedBeneficiary.PlatformsViews.DonationFormCaption = $"<p>{name} enjoys reading and hopes to become a doctor one day.</p>";
        publishedBeneficiary.PlatformsViews.DonationFormProfile = $"<p>{name} enjoys reading and hopes to become a doctor one day.</p>";

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
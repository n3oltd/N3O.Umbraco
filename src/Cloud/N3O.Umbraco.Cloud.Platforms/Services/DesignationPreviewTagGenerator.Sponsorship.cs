using Humanizer;
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

public class SponsorshipDesignationPreviewTagGenerator : DesignationPreviewTagGenerator {
    private readonly ILookups _lookups;

    public SponsorshipDesignationPreviewTagGenerator(ICdnClient cdnClient,
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
        _lookups = lookups;
    }
    
    protected override DesignationType DesignationType => DesignationTypes.Sponsorship;
    
    protected override void PopulatePublishedDesignation(IReadOnlyDictionary<string, object> content,
                                                         PublishedDesignation publishedDesignation) {
        var scheme = GetSponsorshipScheme(content);
        
        var publishedSponsorshipDesignation = new PublishedSponsorshipDesignation();
        publishedSponsorshipDesignation.Scheme = new PublishedDesignationSponsorshipScheme();
        publishedSponsorshipDesignation.Scheme.Id = scheme.Id;
        publishedSponsorshipDesignation.Components = scheme.Components.OrEmpty().Select(ToPublishedSponsorshipComponent).ToList();
        publishedSponsorshipDesignation.AllowedDurations = scheme.AllowedDurations.OrEmpty().Select(ToPublishedCommitmentDuration).ToList();
        
        publishedDesignation.Sponsorship = publishedSponsorshipDesignation;
    }
    
    protected override IFundDimensionOptions GetFundDimensionOptions(IReadOnlyDictionary<string, object> content) {
        var scheme = GetSponsorshipScheme(content);
        
        return scheme?.FundDimensionOptions;
    }

    protected override IEnumerable<PublishedGiftType> GetPublishedSuggestedGiftTypes(IReadOnlyDictionary<string, object> content) {
        return GetSponsorshipScheme(content)?.AllowedGivingTypes.Select(x => x.ToGiftType().ToEnum<PublishedGiftType>().GetValueOrThrow());
    }
    
    private SponsorshipScheme GetSponsorshipScheme(IReadOnlyDictionary<string, object> content) {
        return GetDataListValue<SponsorshipScheme>(content, AliasHelper<SponsorshipDesignationContent>.PropertyAlias(x => x.Scheme));
    }
    
    private PublishedCommitmentDuration ToPublishedCommitmentDuration(SponsorshipDuration sponsorshipDuration) {
        var commitmentDuration = new PublishedCommitmentDuration();
        commitmentDuration.Id = sponsorshipDuration.Id;
        commitmentDuration.Name = sponsorshipDuration.Name;
        commitmentDuration.Months = sponsorshipDuration.Months;
        
        return commitmentDuration;
    }

    private PublishedSponsorshipSchemeComponent ToPublishedSponsorshipComponent(SponsorshipComponent sponsorshipComponent) {
        var publishedSponsorshipComponent = new PublishedSponsorshipSchemeComponent();
        
        publishedSponsorshipComponent.Name = sponsorshipComponent.Name;
        publishedSponsorshipComponent.Required = sponsorshipComponent.Mandatory;
        publishedSponsorshipComponent.Pricing = sponsorshipComponent.Pricing.IfNotNull(Mapper.Map<IPricing, PublishedPricing>);
        
        return publishedSponsorshipComponent;
    }

    protected override void PopulateAdditionalData(Dictionary<string, object> previewData,
                                                   PublishedDonationForm publishedDonationForm) {
        var sponsorshipScheme = _lookups.FindById<SponsorshipScheme>(publishedDonationForm.Designation.Sponsorship.Scheme.Id);
        
        previewData["previewBeneficiaries"] = GetBeneficiaries(sponsorshipScheme);
        previewData["sponsorshipSchemes"] = GetSponsorshipSchemes(sponsorshipScheme);
    }

    private object GetSponsorshipSchemes(SponsorshipScheme sponsorshipScheme) {
        var publishedSponsorshipScheme = new PublishedSponsorshipScheme();
        publishedSponsorshipScheme.Id = sponsorshipScheme.Id;
        publishedSponsorshipScheme.Name = sponsorshipScheme.Name;
        publishedSponsorshipScheme.AvailableLocations = sponsorshipScheme.AvailableLocations.ToList();
        
        publishedSponsorshipScheme.FundDimensionOptions = new PublishedSponsorshipSchemeFundDimensionOptions();
        publishedSponsorshipScheme.FundDimensionOptions.Dimension1 = sponsorshipScheme.FundDimensionOptions.Dimension1?.Select(x => x.Name).ToList();
        publishedSponsorshipScheme.FundDimensionOptions.Dimension2 = sponsorshipScheme.FundDimensionOptions.Dimension2?.Select(x => x.Name).ToList();
        publishedSponsorshipScheme.FundDimensionOptions.Dimension3 = sponsorshipScheme.FundDimensionOptions.Dimension3?.Select(x => x.Name).ToList();
        publishedSponsorshipScheme.FundDimensionOptions.Dimension4 = sponsorshipScheme.FundDimensionOptions.Dimension4?.Select(x => x.Name).ToList();

        return publishedSponsorshipScheme;
    }

    private IEnumerable<PublishedBeneficiary> GetBeneficiaries(SponsorshipScheme sponsorshipScheme) {
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
        
        publishedBeneficiary.EmbedViews = new PublishedEmbedViews();
        publishedBeneficiary.EmbedViews.Caption = $"<p>{name} enjoys reading and hopes to become a doctor one day.</p>";
        
        return publishedBeneficiary.Yield();
    }

    private IEnumerable<PublishedBeneficiaryComponent> GetComponents(SponsorshipScheme sponsorshipScheme) {
        var component = new PublishedBeneficiaryComponent();
        component.Name = sponsorshipScheme.Components.First().Name;
        component.Quantity = 1;
        component.Price = new PublishedPrice();
        component.Price.Amount = (double) sponsorshipScheme.Components.First().Pricing.Price.Amount;
        component.Price.Locked = sponsorshipScheme.Components.First().Pricing.Price.Locked;
        
        return component.Yield();
    }
}
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
               publishedValueFallback) { }
    
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

    private PublishedSponsorshipComponent ToPublishedSponsorshipComponent(SponsorshipComponent sponsorshipComponent) {
        var publishedSponsorshipComponent = new PublishedSponsorshipComponent();
        
        publishedSponsorshipComponent.Name = sponsorshipComponent.Name;
        publishedSponsorshipComponent.Required = sponsorshipComponent.Mandatory;
        publishedSponsorshipComponent.Pricing = sponsorshipComponent.Pricing.IfNotNull(Mapper.Map<IPricing, PublishedPricing>);
        
        return publishedSponsorshipComponent;
    }
}
using MuslimHands.Website.Connect.Clients;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using System.Collections.Generic;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Cloud.Platforms.Models.Connect.Elements;

public class PublishedDonateButtonMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;

    public PublishedDonateButtonMapping(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DonateButtonElementContent, PublishedDonateButton>((_, _) => new PublishedDonateButton(), Map);
    }
    
    private void Map(DonateButtonElementContent src, PublishedDonateButton dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Type = ElementType.DonateButton;
        dest.Analytics = ctx.Map<IEnumerable<DataListItem>, PublishedAnalyticsParameters>(src.AnalyticsTags);

        if (src.Campaign.HasValue()) {
            var defaultDesignationForCampaign = src.Campaign
                                                   .Descendants()
                                                   .Where(x => x.IsComposedOf(AliasHelper<Designation>.ContentTypeAlias()))
                                                   .As<IDesignation>()
                                                   .First();
            
            dest.Campaign = ctx.Map<ICampaign, PublishedCampaignSummary>(src.Campaign);
            dest.Designation = ctx.Map<IDesignation, PublishedDesignation>(defaultDesignationForCampaign);
        } else {
            var defaultCampaign = _contentLocator.Single<Campaigns>().Children.First().As<Campaign>();
            var defaultDesignationForCampaign = defaultCampaign.Descendants()
                                                               .Where(x => x.IsComposedOf(AliasHelper<Designation>.ContentTypeAlias()))
                                                               .As<IDesignation>()
                                                               .First();
            
            dest.Campaign = ctx.Map<ICampaign, PublishedCampaignSummary>(defaultCampaign);
            dest.Designation = ctx.Map<IDesignation, PublishedDesignation>(defaultDesignationForCampaign);
        }
    }
}
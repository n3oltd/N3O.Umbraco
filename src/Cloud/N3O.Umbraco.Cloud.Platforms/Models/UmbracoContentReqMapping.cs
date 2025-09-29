using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class UmbracoContentReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PlatformsContent, UmbracoContentReq>((_, _) => new UmbracoContentReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PlatformsContent src, UmbracoContentReq dest, MapperContext ctx) {
        dest.Campaigns = MapCampaigns(ctx, src);
        dest.DonateButtons = MapDonateButtons(ctx, src);
        dest.DonationForms = MapDonationForms(ctx, src);
        dest.DonateMenu = ctx.Map<PlatformsContent, PublishedDonateMenu>(src);
    }
    
    private List<UmbracoContentRevisionReqPublishedCampaign> MapCampaigns(MapperContext ctx, PlatformsContent platformsContent) {
        var campaignsReq = new List<UmbracoContentRevisionReqPublishedCampaign>();
        
        foreach (var campaign in platformsContent.Campaigns.Where(x => x.Content().Children.HasAny())) {
            var req = new UmbracoContentRevisionReqPublishedCampaign();
            req.Content = ctx.Map<CampaignContent, PublishedCampaign>(campaign);
            req.Version = 1;
            
            campaignsReq.Add(req);
        }

        return campaignsReq;
    }
    
    private List<UmbracoContentRevisionReqPublishedDonateButton> MapDonateButtons(MapperContext ctx,
                                                                                  PlatformsContent platformsContent) {
        var reqs = new List<UmbracoContentRevisionReqPublishedDonateButton>();
        
        foreach (var element in platformsContent.Elements.Where(x => x.Type == ElementTypes.DonateButton)) {
            var req = new UmbracoContentRevisionReqPublishedDonateButton();
            req.Content = ctx.Map<ElementContent, PublishedDonateButton>(element);
            req.Version = 1;
            
            reqs.Add(req);
        }

        return reqs;
    }
    
    private List<UmbracoContentRevisionReqPublishedDonationForm> MapDonationForms(MapperContext ctx,
                                                                                  PlatformsContent platformsContent) {
        var reqs = new List<UmbracoContentRevisionReqPublishedDonationForm>();
        
        foreach (var element in platformsContent.Elements.Where(x => x.Type == ElementTypes.DonationForm)) {
            var req = new UmbracoContentRevisionReqPublishedDonationForm();
            req.Content = ctx.Map<ElementContent, PublishedDonationForm>(element);
            req.Version = 1;
            
            reqs.Add(req);
        }

        return reqs;
    }
}
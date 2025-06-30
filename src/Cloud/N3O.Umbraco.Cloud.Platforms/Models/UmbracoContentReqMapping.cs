using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
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
        dest.AddressEntry = ctx.Map<AddressEntryContent, UmbracoAddressEntryReq>(src.Settings.AccountEntry.Address);
        dest.BannerAdverts = ctx.Map<BannerAdvertsContent, PublishedBannerAdverts>(src.Settings.PaymentsSettings.BannerAdverts);
        dest.Campaigns = MapCampaigns(ctx, src);
        dest.ConsentEntry = ctx.Map<ConsentEntryContent, UmbracoConsentEntryReq>(src.Settings.AccountEntry.Consent);
        dest.DonateButtons = MapDonateButtons(ctx, src);
        dest.DonationForms = MapDonationForms(ctx, src);
        dest.DonateMenu = ctx.Map<PlatformsContent, PublishedDonateMenu>(src);
        dest.FundStructure = ctx.Map<FundStructureContent, UmbracoFundStructureReq>(src.Settings.FundStructure);
        dest.OrganizationInfo = ctx.Map<OrganizationInfoContent, PublishedOrganizationInfo>(src.Settings.OrganizationInfo);
        dest.PaymentTerms =  ctx.Map<PaymentTermsContent, PublishedPaymentTerms>(src.Settings.PaymentsSettings.Terms);
        dest.Terminology = ctx.Map<TerminologiesContent, PublishedTerminology>(src.Settings.Terminologies);
        dest.Theme = ctx.Map<ThemeSettingsContent, PublishedTheme>(src.Settings.Build.Theme);
        dest.Tracking =  ctx.Map<TrackingContent, PublishedTracking>(src.Settings.Tracking);
    }
    
    private List<UmbracoContentRevisionReqPublishedCampaign> MapCampaigns(MapperContext ctx, PlatformsContent platformsContent) {
        var campaignsReq = new List<UmbracoContentRevisionReqPublishedCampaign>();
        
        foreach (var campaign in platformsContent.Campaigns) {
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
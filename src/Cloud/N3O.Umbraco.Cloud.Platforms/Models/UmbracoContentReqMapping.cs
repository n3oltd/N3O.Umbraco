using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Content.Settings;
using N3O.Umbraco.Cloud.Platforms.Content.Settings.Tracking;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class UmbracoContentReqMapping : IMapDefinition {
    //private readonly IContentLocator _contentLocator;

    public UmbracoContentReqMapping(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PlatformsContent, UmbracoContentReq>((_, _) => new UmbracoContentReq(), Map);
    }

    private void Map(PlatformsContent src, UmbracoContentReq dest, MapperContext ctx) {
        dest.DonateMenu = ctx.Map<PlatformsContent, PublishedDonateMenu>(src);
        dest.FundStructure = ctx.Map<FundStructureContent, UmbracoFundStructureReq>(src.Settings.FundStructure);
        dest.OrganizationInfo = ctx.Map<OrganizationInfoContent, PublishedOrganizationInfo>(src.Settings.OrganizationInfo);
        dest.PaymentTerms =  ctx.Map<PaymentTermsContent, PublishedPaymentTerms>(src.Settings.PaymentsSettings.PaymentTerms);
        dest.Terminology = ctx.Map<TerminologiesContent, PublishedTerminology>(src.Settings.Terminologies);
        dest.Tracking =  ctx.Map<TrackingContent, PublishedTracking>(src.Settings.Tracking);
        dest.BannerAdverts = ctx.Map<BannerAdvertsContent, PublishedBannerAdverts>(src.Settings.PaymentsSettings.BannerAdverts);
        dest.AddressEntry = ctx.Map<AddressEntryContent, UmbracoAddressEntryReq>(src.Settings.DataEntry.Address);
        dest.NameEntry = ctx.Map<NameEntryContent, UmbracoNameEntryReq>(src.Settings.DataEntry.Name);
        dest.ConsentEntry = ctx.Map<ConsentEntryContent, UmbracoConsentEntryReq>(src.Settings.DataEntry.Consent);
        dest.Theme = ctx.Map<ThemeSettingsContent, PublishedTheme>(src.Settings.BuildSettings.Theme);
        
        dest.Campaigns = GetCampaignsReq(ctx, src);
        dest.DonateButtons = GetDonateButtons(ctx, src);
        dest.DonationForms = GetDonationForms(ctx, src);
    }
    
    private PublishedBannerAdverts GetBannerAdverts(MapperContext ctx, PlatformsContent platformsContent) {
        var adverts = platformsContent.Settings.PaymentsSettings.BannerAdverts.Adverts;

        var req = new PublishedBannerAdverts();
        req.Adverts = adverts.OrEmpty().Select(ctx.Map<BannerAdvertContent, PublishedBannerAdvert>).ToList();
        
        return req;
    }
    
    private List<UmbracoContentRevisionReqPublishedCampaign> GetCampaignsReq(MapperContext ctx, PlatformsContent platformsContent) {
        var campaignsReq = new List<UmbracoContentRevisionReqPublishedCampaign>();
        
        foreach (var campaign in platformsContent.Campaigns) {
            var req = new UmbracoContentRevisionReqPublishedCampaign();
            req.Content = ctx.Map<CampaignContent, PublishedCampaign>(campaign);
            req.Version = 1;
            
            campaignsReq.Add(req);
        }

        return campaignsReq;
    }
    
    private List<UmbracoContentRevisionReqPublishedDonateButton> GetDonateButtons(MapperContext ctx,
                                                                                  PlatformsContent platformsContent) {
        var donateButtons = platforms.DescendantsOfType(AliasHelper<DonateButtonElement>.ContentTypeAlias())
        var donateButtonsReq = new List<UmbracoContentRevisionReqPublishedDonateButton>();
        
        foreach (var donateButton in donateButtons) {
            var req = new UmbracoContentRevisionReqPublishedDonateButton();
            req.Content = ctx.Map<DonateButtonElement, PublishedDonateButton>(donateButton);
            req.Version = 1;
            
            donateButtonsReq.Add(req);
        }

        return donateButtonsReq;
    }
    
    private List<PublishedDonationFormUmbracoContentRevisionReq> GetDonationForms(MapperContext ctx,
                                                                                  Platforms platforms) {
        var donationForms = platforms.DescendantsOfType(AliasHelper<DonationFormElement>.ContentTypeAlias())
                                     .As<DonationFormElement>();
        
        var donationFormsReq = new List<PublishedDonationFormUmbracoContentRevisionReq>();
        
        foreach (var donateButton in donationForms) {
            var req = new PublishedDonationFormUmbracoContentRevisionReq();
            req.Content = ctx.Map<DonationFormElement, PublishedDonationForm>(donateButton);
            req.Version = 1;
            
            donationFormsReq.Add(req);
        }

        return donationFormsReq;
    }
}
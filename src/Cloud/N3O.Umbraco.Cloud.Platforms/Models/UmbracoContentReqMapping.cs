using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class UmbracoContentReqMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;

    public UmbracoContentReqMapping(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PlatformsContent, UmbracoContentReq>((_, _) => new UmbracoContentReq(), Map);
    }

    private void Map(PlatformsContent src, UmbracoContentReq dest, MapperContext ctx) {
        dest.DonateMenu = ctx.Map<Platforms, PublishedDonateMenu>(src);
        dest.BannerAdverts = GetBannerAdverts(ctx);
        dest.Campaigns = GetCampaignsReq(ctx, src);
        dest.DonateButtons = GetDonateButtons(ctx, src);
        dest.DonationForms = GetDonationForms(ctx, src);
        dest.FundStructure = GetFundStructure(ctx);
        dest.OrganizationInfo = GetOrganizationInfo(ctx);
        dest.PaymentTerms = GetPaymentTerms(ctx);
        dest.Terminology = GetTerminology(ctx);
        dest.Tracking = GetTracking(ctx);
        dest.AddressEntry = GetAddressEntry();
        dest.NameEntry = GetNameEntry();
        dest.ConsentEntry = GetConsentEntry();
        dest.Theme = GetThemeSettings(ctx);
    }
    
    private PublishedBannerAdverts GetBannerAdverts(MapperContext ctx) {
        var platformsBannerAdverts = _contentLocator.Single<PlatformsBannerAdverts>();
        var adverts = platformsBannerAdverts.Children<PlatformsBannerAdvert>();

        var req = new PublishedBannerAdverts();
        req.Adverts = adverts.OrEmpty().Select(ctx.Map<PlatformsBannerAdvert, PublishedBannerAdvert>).ToList();
        
        return req;
    }
    
    private List<PublishedCampaignUmbracoContentRevisionReq> GetCampaignsReq(MapperContext ctx, Platforms platforms) {
        var campaigns = platforms.Descendants()
                                 .Where(x => x.IsComposedOf(AliasHelper<Campaign>.ContentTypeAlias()))
                                 .As<ICampaign>();

        var campaignsReq = new List<PublishedCampaignUmbracoContentRevisionReq>();
        
        foreach (var campaign in campaigns) {
            var req = new PublishedCampaignUmbracoContentRevisionReq();
            req.Content = ctx.Map<ICampaign, PublishedCampaign>(campaign);
            req.Version = 1;
            
            campaignsReq.Add(req);
        }

        return campaignsReq;
    }
    
    private List<PublishedDonateButtonUmbracoContentRevisionReq> GetDonateButtons(MapperContext ctx,
                                                                                  Platforms platforms) {
        var donateButtons = platforms.DescendantsOfType(AliasHelper<DonateButtonElement>.ContentTypeAlias())
                                     .As<DonateButtonElement>();
        
        var donateButtonsReq = new List<PublishedDonateButtonUmbracoContentRevisionReq>();
        
        foreach (var donateButton in donateButtons) {
            var req = new PublishedDonateButtonUmbracoContentRevisionReq();
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
    
    private UmbracoFundStructureReq GetFundStructure(MapperContext ctx) {
        var fundDimensions = _contentLocator.Single<PlatformsFundStructure>();

        return ctx.Map<PlatformsFundStructure, UmbracoFundStructureReq>(fundDimensions);
    }
    
    private PublishedOrganizationInfo GetOrganizationInfo(MapperContext ctx) {
        var organisationDataEntrySettings = _contentLocator.Single<PlatformsOrganizationInfo>();

        return ctx.Map<PlatformsOrganizationInfo, PublishedOrganizationInfo>(organisationDataEntrySettings);
    }
    
    private PublishedPaymentTerms GetPaymentTerms(MapperContext ctx) {
        var paymentTermsDataEntrySettings = _contentLocator.Single<PlatformsPaymentTerms>();

        return ctx.Map<PlatformsPaymentTerms, PublishedPaymentTerms>(paymentTermsDataEntrySettings);
    }
    
    private PublishedTerminology GetTerminology(MapperContext ctx) {
        var terminologySettings = _contentLocator.Single<PlatformsTerminologies>();

        return ctx.Map<PlatformsTerminologies, PublishedTerminology>(terminologySettings);
    }
    
    private PublishedTracking GetTracking(MapperContext ctx) {
        var platformsTracking = _contentLocator.Single<PlatformsTracking>();

        return ctx.Map<PlatformsTracking, PublishedTracking>(platformsTracking);
    }
    
    private UmbracoConsentEntryReq GetConsentEntry() {
        var consent = _contentLocator.Single<PlatformsConsent>();
        
        var privacyUrl = consent.PrivacyUrl.Content?.AbsoluteUrl() ?? consent.PrivacyUrl.Url;
        
        var req = new UmbracoConsentEntryReq();
        req.ConsentText = consent.ConsentText;
        req.PrivacyText = consent.PrivacyText;
        req.PrivacyUrl = new Uri(privacyUrl);
        
        return req;
    }

    private UmbracoNameEntryReq GetNameEntry() {
        var nameEntry = _contentLocator.Single<PlatformsNameEntry>();
        
        var req = new UmbracoNameEntryReq();
        req.Layout = (NameLayout) Enum.Parse(typeof(NameLayout), nameEntry.Layout.Id, true);
        
        return req;
    }

    private UmbracoAddressEntryReq GetAddressEntry() {
        var addressEntry = _contentLocator.Single<PlatformsAddressEntry>();
        
        var req = new UmbracoAddressEntryReq();
        req.AddressLookupApiKey = addressEntry.LookupApiKey;
        req.Layout = (AddressLayout) Enum.Parse(typeof(AddressLayout), addressEntry.Layout.Id, true);
        
        return req;
    }
    
    private PublishedTheme GetThemeSettings(MapperContext ctx) {
        var themeSettings = _contentLocator.Single<PlatformsThemeSettings>();
        
        return ctx.Map<PlatformsThemeSettings, PublishedTheme>(themeSettings);
    }
}
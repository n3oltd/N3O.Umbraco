using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class UmbracoSubscriptionSettingsReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SettingsContent, UmbracoSubscriptionSettingsReq>((_, _) => new UmbracoSubscriptionSettingsReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(SettingsContent src, UmbracoSubscriptionSettingsReq dest, MapperContext ctx) {
        dest.AddressEntry = ctx.Map<AddressEntryContent, UmbracoAddressEntryReq>(src.AccountEntry.Address);
        dest.BannerAdverts = ctx.Map<BannerAdvertsContent, PublishedBannerAdverts>(src.PaymentsSettings.BannerAdverts);
        dest.ConsentEntry = ctx.Map<ConsentEntryContent, UmbracoConsentEntryReq>(src.AccountEntry.Consent);
        dest.FundStructure = ctx.Map<FundStructureContent, UmbracoFundStructureReq>(src.FundStructure);
        dest.OrganizationInfo = ctx.Map<OrganizationInfoContent, PublishedOrganizationInfo>(src.OrganizationInfo);
        dest.PaymentTerms =  ctx.Map<PaymentTermsContent, PublishedPaymentTerms>(src.PaymentsSettings.Terms);
        dest.Terminology = ctx.Map<TerminologiesContent, PublishedTerminology>(src.Terminologies);
        dest.Theme = ctx.Map<ThemeSettingsContent, PublishedTheme>(src.Build.Theme);
        dest.Tracking =  ctx.Map<TrackingContent, PublishedTracking>(src.Tracking);
    }
}
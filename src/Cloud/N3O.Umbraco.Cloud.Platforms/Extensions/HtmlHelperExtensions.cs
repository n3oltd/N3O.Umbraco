using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class HtmlHelperExtensions {
    public static async Task<HtmlString> EmbedElementAsync(this IHtmlHelper helper,
                                                           IPlatformsPageAccessor platformsPageAccessor,
                                                           Element element) {
        var embedCode = element.EmbedCode;
        var platformsPage = await platformsPageAccessor.GetAsync();
        var page = platformsPage?.Page;

        if (page != null) {
            if (element.ElementKind == ElementKind.DonationButton) {
                string idAttribute;
                ElementKind elementKind;
            
                if (page.Kind == PublishedFileKinds.CampaignPage) {
                    idAttribute = page.GetCampaignId();
                    elementKind = ElementKind.DonationButtonCampaign;
                } else if (page.Kind == PublishedFileKinds.OfferingPage) {
                    idAttribute = page.GetOfferingId();
                    elementKind = ElementKind.DonationButtonOffering;
                } else {
                    throw UnrecognisedValueException.For(page.Kind);
                }
            
                embedCode = $"""<n3o-donation-button element-id="{idAttribute}" element-kind="{elementKind.ToEnumString()}"></n3o-donation-button>""";
            } else if (element.ElementKind == ElementKind.DonationForm) {
                ElementKind elementKind;
                string idAttribute;
            
                if (page.Kind == PublishedFileKinds.CampaignPage) {
                    idAttribute = page.GetCampaignId();
                    elementKind = ElementKind.DonationFormCampaign;
                } else if (page.Kind == PublishedFileKinds.OfferingPage) {
                    idAttribute = page.GetOfferingId();
                    elementKind = ElementKind.DonationFormOffering;
                } else {
                    throw UnrecognisedValueException.For(page.Kind);
                }
            
                embedCode = $"""<n3o-donation-form element-id="{idAttribute}" element-kind="{elementKind.ToEnumString()}"></n3o-donation-form>""";
            } else if (element.ElementKind == ElementKind.DonationPopup) {
                string idAttribute;
                ElementKind elementKind;
            
                if (page.Kind == PublishedFileKinds.CampaignPage) {
                    idAttribute = page.GetCampaignId();
                    elementKind = ElementKind.DonationPopupCampaign;
                } else if (page.Kind == PublishedFileKinds.OfferingPage) {
                    idAttribute = page.GetOfferingId();
                    elementKind = ElementKind.DonationPopupOffering;
                } else {
                    throw UnrecognisedValueException.For(page.Kind);
                }
            
                embedCode = $"""<n3o-donation-popup element-id="{idAttribute}" element-kind="{elementKind.ToEnumString()}"></n3o-donation-popup>""";
            }
        }
        
        return embedCode.ToHtmlString();
    }
}
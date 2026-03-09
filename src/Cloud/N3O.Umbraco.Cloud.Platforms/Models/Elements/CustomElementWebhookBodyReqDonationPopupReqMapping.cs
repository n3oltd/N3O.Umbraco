using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CustomElementWebhookBodyReqDonationPopupReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ElementContent, CustomElementWebhookBodyReqDonationPopupReq>((_, _) => new CustomElementWebhookBodyReqDonationPopupReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(ElementContent src, CustomElementWebhookBodyReqDonationPopupReq dest, MapperContext ctx) {
        dest.Action = WebhookSyncAction.AddOrUpdate;
        dest.Id = src.Key.ToString();

        dest.AddOrUpdate = new CustomElementReqDonationPopupReq();
        dest.AddOrUpdate.Name = src.Content().Name;
            
        dest.AddOrUpdate.Data = new DonationPopupReq();
        dest.AddOrUpdate.Data.TimeDelaySeconds = src.DonationPopup.TimeDelaySeconds;
        dest.AddOrUpdate.Data.FormState = ctx.Map<ElementContent, DonationFormStateReq>(src);
        
        if (src.Content().IsPublished()) {
            dest.AddOrUpdate.Activate = true;
        }
    }
}
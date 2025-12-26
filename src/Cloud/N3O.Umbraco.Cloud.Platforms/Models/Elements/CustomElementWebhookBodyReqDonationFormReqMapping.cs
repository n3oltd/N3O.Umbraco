using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CustomElementWebhookBodyReqDonationFormReqMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;

    public CustomElementWebhookBodyReqDonationFormReqMapping(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ElementContent, CustomElementWebhookBodyReqDonationFormReq>((_, _) => new CustomElementWebhookBodyReqDonationFormReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(ElementContent src, CustomElementWebhookBodyReqDonationFormReq dest, MapperContext ctx) {
        dest.Action = WebhookSyncAction.AddOrUpdate;
        dest.Id = src.Key.ToString();

        dest.AddOrUpdate = new CustomElementReqDonationFormReq();
        dest.AddOrUpdate.Name = src.Content().Name;
        
        dest.AddOrUpdate.Data = new DonationFormReq();
        dest.AddOrUpdate.Data.FormState = ctx.Map<ElementContent, DonationFormStateReq>(src);
        
        if (src.Content().IsPublished()) {
            dest.AddOrUpdate.Activate = true;
        }
    }
}
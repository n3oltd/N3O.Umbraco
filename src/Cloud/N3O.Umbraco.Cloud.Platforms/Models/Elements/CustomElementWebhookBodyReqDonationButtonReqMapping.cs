using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;
using DonationButtonAction = N3O.Umbraco.Cloud.Platforms.Clients.DonationButtonAction;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CustomElementWebhookBodyReqDonationButtonReqMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;

    public CustomElementWebhookBodyReqDonationButtonReqMapping(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ElementContent, CustomElementWebhookBodyReqDonationButtonReq>((_, _) => new CustomElementWebhookBodyReqDonationButtonReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(ElementContent src, CustomElementWebhookBodyReqDonationButtonReq dest, MapperContext ctx) {
        dest.Action = WebhookSyncAction.AddOrUpdate;
        dest.Id = src.Key.ToString();

        dest.AddOrUpdate = new CustomElementReqDonationButtonReq();
        dest.AddOrUpdate.Name = src.Content().Name;
            
        dest.AddOrUpdate.Data = new DonationButtonReq();
        dest.AddOrUpdate.Data.Text = src.DonationButton.Text;
        dest.AddOrUpdate.Data.Action = src.DonationButton.Action.ToEnum<DonationButtonAction>() ?? DonationButtonAction.AddToCart;
        dest.AddOrUpdate.Data.FormState = ctx.Map<ElementContent, DonationFormStateReq>(src);
        
        if (src.Content().IsPublished()) {
            dest.AddOrUpdate.Activate = true;
        }
    }
}
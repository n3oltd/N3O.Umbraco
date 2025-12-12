using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;
using ElementType = N3O.Umbraco.Cloud.Platforms.Clients.ElementType;
using DonationButtonAction = N3O.Umbraco.Cloud.Platforms.Clients.DonationButtonAction;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class ElementWebhookBodyReqMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;

    public ElementWebhookBodyReqMapping(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ElementContent, ElementWebhookBodyReq>((_, _) => new ElementWebhookBodyReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(ElementContent src, ElementWebhookBodyReq dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Action = WebhookSyncAction.AddOrUpdate;

        dest.AddOrUpdate = new CreateElementReq();
        dest.AddOrUpdate.Name = src.Content().Name;
        dest.AddOrUpdate.Type = src.Type.ToEnum<ElementType>();
            
        if (src.Type == ElementTypes.DonationButton) {
            dest.AddOrUpdate.DonationButton = new DonationButtonElementReq();
            dest.AddOrUpdate.DonationButton.Text = src.DonationButton.Text;
            dest.AddOrUpdate.DonationButton.Action = src.DonationButton.Action.ToEnum<DonationButtonAction>();
            dest.AddOrUpdate.DonationButton.FormState = ctx.Map<ElementContent, DonationFormStateReq>(src);
        } else if (src.Type == ElementTypes.DonationForm) {
            dest.AddOrUpdate.DonationForm = new DonationFormElementReq();
            dest.AddOrUpdate.DonationForm.FormState = ctx.Map<ElementContent, DonationFormStateReq>(src);
        } else {
            throw UnrecognisedValueException.For(src.Type);
        }
    }
}
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using ElementType = N3O.Umbraco.Cloud.Platforms.Clients.ElementType;
using DonateButtonAction = N3O.Umbraco.Cloud.Platforms.Clients.DonateButtonAction;

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
        dest.AddOrUpdate.Tags = src.Tags.ToPublishedTagCollection();

        if (src.Campaign.HasValue()) {
            dest.AddOrUpdate.CampaignId = src.Campaign.Key.ToString();
            dest.AddOrUpdate.OfferingId = src.Campaign.DefaultOffering.Key.ToString();
        } else if (src.DonateButton.HasValue(x => x.Offering)) {
            var campaign = src.DonateButton.Offering.Content().Parent.As<CampaignContent>();
            
            dest.AddOrUpdate.CampaignId = campaign.Key.ToString();
            dest.AddOrUpdate.OfferingId = src.Campaign.DefaultOffering.Key.ToString();
        } else {
            var defaultCampaign = _contentLocator.Single<PlatformsContent>().Campaigns.First();
            
            dest.AddOrUpdate.CampaignId = defaultCampaign.Key.ToString();
            dest.AddOrUpdate.OfferingId = defaultCampaign.DefaultOffering.Key.ToString();
        }

        if (src.Type == ElementTypes.DonateButton) {
            dest.AddOrUpdate.DonateButton = new DonateButtonElementReq();
            dest.AddOrUpdate.DonateButton.Text = src.DonateButton.Text;
            dest.AddOrUpdate.DonateButton.Amount = (double) src.DonateButton.Amount;
            dest.AddOrUpdate.DonateButton.Action = src.DonateButton.Action.ToEnum<DonateButtonAction>();
        }

        dest.AddOrUpdate.ElementOptions = new ElementOptionsReq();
        dest.AddOrUpdate.ElementOptions.Dimension1 = src.DonateButton.Dimension1?.Name;
        dest.AddOrUpdate.ElementOptions.Dimension2 = src.DonateButton.Dimension2?.Name;
        dest.AddOrUpdate.ElementOptions.Dimension3 = src.DonateButton.Dimension3?.Name;
        dest.AddOrUpdate.ElementOptions.Dimension4 = src.DonateButton.Dimension4?.Name;
        
        
    }
}
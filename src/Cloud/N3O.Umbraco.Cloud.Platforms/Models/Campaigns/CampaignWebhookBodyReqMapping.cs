using N3O.Umbraco.Cloud.Clients.Platforms;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CampaignWebhookBodyReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CampaignContent, CampaignWebhookBodyReq>((_, _) => new CampaignWebhookBodyReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(CampaignContent src, CampaignWebhookBodyReq dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Action = WebhookSyncAction.AddOrUpdate;

        dest.Add = ctx.Map<CampaignContent, CreateCampaignReq>(src);
        dest.Update = ctx.Map<CampaignContent, UpdateCampaignReq>(src);
    }
}
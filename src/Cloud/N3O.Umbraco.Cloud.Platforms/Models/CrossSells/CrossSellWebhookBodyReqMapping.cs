using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CrossSellWebhookBodyReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CrossSellContent, CrossSellWebhookBodyReq>((_, _) => new CrossSellWebhookBodyReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(CrossSellContent src, CrossSellWebhookBodyReq dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Action = WebhookSyncAction.AddOrUpdate;

        dest.Add = ctx.Map<CrossSellContent, CreateCrossSellReq>(src);
        dest.Update = ctx.Map<CrossSellContent, UpdateCrossSellReq>(src);
    }
}

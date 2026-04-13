using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class QurbaniSeasonWebhookBodyReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<QurbaniSeasonContent, QurbaniSeasonWebhookBodyReq>((_, _) => new QurbaniSeasonWebhookBodyReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(QurbaniSeasonContent src, QurbaniSeasonWebhookBodyReq dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Action = WebhookSyncAction.AddOrUpdate;

        dest.AddOrUpdate = ctx.Map<QurbaniSeasonContent, QurbaniSeasonReq>(src);
    }
}
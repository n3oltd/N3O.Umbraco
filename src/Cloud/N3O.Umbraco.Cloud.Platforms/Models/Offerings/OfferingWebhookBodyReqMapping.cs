using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Media;
using Slugify;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class OfferingWebhookBodyReqMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;
    private readonly ISlugHelper _slugHelper;

    public OfferingWebhookBodyReqMapping(IMediaUrl mediaUrl, ISlugHelper slugHelper) {
        _mediaUrl = mediaUrl;
        _slugHelper = slugHelper;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<OfferingContent, OfferingWebhookBodyReq>((_, _) => new OfferingWebhookBodyReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(OfferingContent src, OfferingWebhookBodyReq dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.CampaignId = src.Content().Parent.Key.ToString();
        dest.Action = WebhookSyncAction.AddOrUpdate;

        dest.Add = ctx.Map<OfferingContent, CreateOfferingReq>(src);
        dest.Update = ctx.Map<OfferingContent, UpdateOfferingReq>(src);

    }
}
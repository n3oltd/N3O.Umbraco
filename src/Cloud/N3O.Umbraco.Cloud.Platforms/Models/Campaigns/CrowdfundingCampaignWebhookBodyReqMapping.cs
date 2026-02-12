using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CrowdfundingCampaignWebhookBodyReqMapping : IMapDefinition {
    public const string PageContentContext = nameof(PageContentContext);
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CrowdfundingCampaignContent, CrowdfundingCampaignWebhookBodyReq>((_, _) => new CrowdfundingCampaignWebhookBodyReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(CrowdfundingCampaignContent src, CrowdfundingCampaignWebhookBodyReq dest, MapperContext ctx) {
        dest.CampaignId = src.Key.ToString();
        dest.Action = WebhookSyncAction.AddOrUpdate;

        dest.AddOrUpdate = GetCrowdfundingCampaignReq(ctx);
    }

    private CrowdfundingCampaignReq GetCrowdfundingCampaignReq(MapperContext ctx) {
        var req = new CrowdfundingCampaignReq();
        req.Enable = true;
        
        req.Template = new ContentReq();
        req.Template.SchemaAlias = CrowdfundingSystemSchema.Sys__crowdfunderPage.ToEnumString();
        
        if (ctx.Items.TryGetValue(PageContentContext, out var value)) {
            var properties = ((IEnumerable<PropertyContentReq>) value).OrEmpty().Where(x => x.EditorHasValue());
            
            req.Template.Properties = properties.ToList();
        }
        
        return req;
    }
}
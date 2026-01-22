using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.EditorJs.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CrowdfundingCampaignWebhookBodyReqMapping : IMapDefinition {
    private readonly ICrowdfunderTemplatePublisher _crowdfunderTemplatePublisher;
    
    public CrowdfundingCampaignWebhookBodyReqMapping(ICrowdfunderTemplatePublisher crowdfunderTemplatePublisher) {
        _crowdfunderTemplatePublisher = crowdfunderTemplatePublisher;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CampaignContent, CrowdfundingCampaignWebhookBodyReq>((_, _) => new CrowdfundingCampaignWebhookBodyReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(CampaignContent src, CrowdfundingCampaignWebhookBodyReq dest, MapperContext ctx) {
        dest.CampaignId = src.Key.ToString();
        dest.Action = WebhookSyncAction.AddOrUpdate;

        dest.AddOrUpdate = GetCrowdfundingCampaignReq(src);
    }

    private CrowdfundingCampaignReq GetCrowdfundingCampaignReq(CampaignContent campaign) {
        var req = new CrowdfundingCampaignReq();
        req.Activate = true;

        req.Template = new ContentReq();
        req.Template.SchemaAlias = CrowdfundingSystemSchema.Sys__crowdfunderPage.ToEnumString();
        req.Template.Properties = _crowdfunderTemplatePublisher.GetContentProperties(campaign.Content()).ToList();
        
        return req;
    }
}
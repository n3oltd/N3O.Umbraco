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
    private readonly IMediaUrl _mediaUrl;
    
    public CrowdfundingCampaignWebhookBodyReqMapping(IMediaUrl mediaUrl) {
        _mediaUrl = mediaUrl;
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
        req.Template.Properties = GetPropertyContentsReq(campaign).ToList();
        
        return req;
    }

    private IEnumerable<PropertyContentReq> GetPropertyContentsReq(CampaignContent campaign) {
        var propertyContents = new List<PropertyContentReq>();

        var body = GetEditorJsPropertyContentReq(AliasHelper<CampaignContent>.PropertyAlias(x => x.NewCrowdfunderTemplateBody), 
                                                 campaign.NewCrowdfunderTemplateBody);
        
        var image = GetImagePropertyContentReq(AliasHelper<CampaignContent>.PropertyAlias(x => x.NewCrowdfunderImage), 
                                               campaign.NewCrowdfunderImage);
        
        propertyContents.Add(body);
        propertyContents.Add(image);
        
        return propertyContents;
    }
    
    private PropertyContentReq GetEditorJsPropertyContentReq(string alias, EditorJsModel editorJs) {
        if (!editorJs.HasValue()) {
            return null;
        }
        
        var blocks = new List<object>();
        editorJs.Blocks.Do(x => blocks.Add(x));
        
        var property = new PropertyContentReq();
        property.Alias = alias;
        property.Editor = PropertyEditor.EditorJs;
        property.EditorJs = new EditorJsContentReq();
        property.EditorJs.Blocks = blocks;
        property.EditorJs.Time = editorJs.LastModified;
        property.EditorJs.Version = editorJs.Version;

        return property;
    }
    
    private PropertyContentReq GetImagePropertyContentReq(string alias, MediaWithCrops media) {
        if (!media.HasValue()) {
            return null;
        }
        
        var property = new PropertyContentReq();
        property.Alias = alias;
        property.Editor = PropertyEditor.ImageSimple;
        property.ImageSimple = media.ToImageSimpleContentReq(_mediaUrl);

        return property;
    }
}
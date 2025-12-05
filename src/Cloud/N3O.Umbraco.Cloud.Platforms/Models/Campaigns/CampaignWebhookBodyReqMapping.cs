using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using NodaTime.Extensions;
using Slugify;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using CampaignType = N3O.Umbraco.Cloud.Platforms.Clients.CampaignType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class CampaignWebhookBodyReqMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;
    private readonly ISlugHelper _slugHelper;

    public CampaignWebhookBodyReqMapping(IMediaUrl mediaUrl, ISlugHelper slugHelper) {
        _mediaUrl = mediaUrl;
        _slugHelper = slugHelper;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CampaignContent, CampaignWebhookBodyReq>((_, _) => new CampaignWebhookBodyReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(CampaignContent src, CampaignWebhookBodyReq dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Action = WebhookSyncAction.AddOrUpdate;
        
        dest.AddOrUpdate = new CreateCampaignReq();
        dest.AddOrUpdate.Type = src.Type.ToEnum<CampaignType>();
        dest.AddOrUpdate.Name = src.Name;
        dest.AddOrUpdate.Slug = _slugHelper.GenerateSlug(src.Name);
        dest.AddOrUpdate.Notes = src.Notes;
        dest.AddOrUpdate.Target = (double) src.Target;
        
        dest.AddOrUpdate.Description = new RichTextContentReq();
        dest.AddOrUpdate.Description.Html = src.Description.ToHtmlString(); 
        dest.AddOrUpdate.Image = src.Image.ToImageSimpleContentReq(_mediaUrl);
        dest.AddOrUpdate.Icon = new SvgContentReq();
        dest.AddOrUpdate.Icon.SourceFile = _mediaUrl.GetMediaUrl(src.Icon, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x)).ToString();
        dest.AddOrUpdate.Tags = src.Tags.ToTagCollectionReq();
        
        /*TODO populate page*/
        //dest.AddOrUpdate.Page = 
        
        if (src.Type == CampaignTypes.Telethon) {
            dest.AddOrUpdate.Telethon = new TelethonCampaignReq();
            
            dest.AddOrUpdate.Telethon.Begin = src.Telethon.BeginAt.ToInstant().ToString();
            dest.AddOrUpdate.Telethon.End = src.Telethon.EndAt.ToInstant().ToString();
        } else if (src.Type == CampaignTypes.ScheduledGiving) {
            dest.AddOrUpdate.ScheduledGiving = new ScheduledGivingCampaignReq();
            //dest.AddOrUpdate.ScheduledGiving.ScheduleId; TODO
        }
    }
}
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
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class UpdateCampaignReqMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;
    private readonly ISlugHelper _slugHelper;

    public UpdateCampaignReqMapping(IMediaUrl mediaUrl, ISlugHelper slugHelper) {
        _mediaUrl = mediaUrl;
        _slugHelper = slugHelper;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CampaignContent, UpdateCampaignReq>((_, _) => new UpdateCampaignReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(CampaignContent src, UpdateCampaignReq dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.Notes = src.Notes;
        dest.Slug = _slugHelper.GenerateSlug(src.Name);
        dest.Target = (double) src.Target;
        
        dest.Description = new RichTextContentReq();
        dest.Description.Html = src.Description.ToHtmlString(); 
        dest.Image = src.Image.ToImageSimpleContentReq(_mediaUrl);
        dest.Icon = new SvgContentReq();
        dest.Icon.SourceFile = _mediaUrl.GetMediaUrl(src.Icon, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x)).ToString();

        dest.Order = new CampaignOrderReq();
        dest.Order.Order = src.Content().Parent.Children.FindIndex(x => x.Id == src.Content().Id);

        dest.Badges = null;

        dest.Page = new ContentReq();
        dest.Page.SchemaAlias = nameof(PlatformsSystemSchema.Sys__campaignPage).ToLower();

        if (src.Content().IsPublished()) {
            dest.Activate = true;
        }
        
        if (src.Type == CampaignTypes.Telethon) {
            dest.Telethon = new TelethonCampaignOptionsReq();
            
            dest.Telethon.Begin = src.Telethon.BeginAt.ToLocalDateTime().ToString("o", null);
            dest.Telethon.End = src.Telethon.EndAt.ToLocalDateTime().ToString("o", null);
        } else if (src.Type == CampaignTypes.ScheduledGiving) {
            dest.ScheduledGiving = new ScheduledGivingCampaignOptionsReq();
            dest.ScheduledGiving.ScheduleId = src.ScheduledGiving.Schedule.Id;
        }
    }
}
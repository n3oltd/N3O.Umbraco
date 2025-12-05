using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using Slugify;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

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
        dest.Action = WebhookSyncAction.AddOrUpdate;

        dest.AddOrUpdate = new CreateOfferingReq();
        dest.AddOrUpdate.CampaignId = src.Content().Parent.Key.ToString();
        dest.AddOrUpdate.Name = src.Name;
        dest.AddOrUpdate.Slug = _slugHelper.GenerateSlug(src.Name);
        dest.AddOrUpdate.Image = src.Image.ToImageSimpleContentReq(_mediaUrl);
        dest.AddOrUpdate.Icon = new SvgContentReq();
        dest.AddOrUpdate.Icon.SourceFile = _mediaUrl.GetMediaUrl(src.Icon, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x)).ToString();
        
        dest.AddOrUpdate.Description = new RichTextContentReq();
        dest.AddOrUpdate.Description.Html = src.Description.ToHtmlString();
        
        dest.AddOrUpdate.DonationForm = ctx.Map<OfferingContent, DonationFormStateReq>(src);
        
        dest.AddOrUpdate.Options = new OfferingOptionsReq();
        dest.AddOrUpdate.Options.AllowCrowdfunding = src.AllowCrowdfunding;
        
        /*TODO add page content*/
        //dest.AddOrUpdate.Page = ;
        
    }
}
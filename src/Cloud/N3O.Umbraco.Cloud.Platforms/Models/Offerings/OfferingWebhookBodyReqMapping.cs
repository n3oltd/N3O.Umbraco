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
        dest.AddOrUpdate.Type = src.Type.ToEnum<OfferingType>();
        dest.AddOrUpdate.Name = src.Name;
        dest.AddOrUpdate.Slug = _slugHelper.GenerateSlug(src.Name);
        dest.AddOrUpdate.SuggestedGiftType = src.SuggestedGiftType.ToEnum<GiftType>();
        dest.AddOrUpdate.FundDimensionsOptions = src.ToOfferingFundDimensionReq();
        dest.AddOrUpdate.Image = src.Image.ToImageSimpleContentReq(_mediaUrl);
        dest.AddOrUpdate.Icon = new SvgContentReq();
        dest.AddOrUpdate.Icon.SourceFile = _mediaUrl.GetMediaUrl(src.Icon, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x)).ToString();
        
        dest.AddOrUpdate.Description = new RichTextContentReq();
        dest.AddOrUpdate.Description.Html = src.Description.ToHtmlString();
        
        /*TODO add page properties*/
        //dest.AddOrUpdate.Page = ;
        
        dest.AddOrUpdate.Fund = src.Fund.IfNotNull(ctx.Map<FundOfferingContent, FundOfferingReq>);
        dest.AddOrUpdate.Feedback = src.Feedback.IfNotNull(ctx.Map<FeedbackOfferingContent, FeedbackOfferingReq>);
        dest.AddOrUpdate.Sponsorship = src.Sponsorship.IfNotNull(ctx.Map<SponsorshipOfferingContent, SponsorshipOfferingReq>);
        
        dest.AddOrUpdate.Options = new OfferingOptionsReq();
        dest.AddOrUpdate.Options.AllowCrowdfunding = src.AllowCrowdfunding;
        
    }
}
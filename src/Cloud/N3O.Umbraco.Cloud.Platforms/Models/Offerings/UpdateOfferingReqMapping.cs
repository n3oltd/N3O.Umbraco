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

public class UpdateOfferingReqMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;
    private readonly ISlugHelper _slugHelper;

    public UpdateOfferingReqMapping(IMediaUrl mediaUrl, ISlugHelper slugHelper) {
        _mediaUrl = mediaUrl;
        _slugHelper = slugHelper;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<OfferingContent, UpdateOfferingReq>((_, _) => new UpdateOfferingReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(OfferingContent src, UpdateOfferingReq dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.Notes = src.Notes;
        dest.Slug = _slugHelper.GenerateSlug(src.Name);
        
        dest.Description = new RichTextContentReq();
        dest.Description.Html = src.Description.ToHtmlString(); 
        dest.Image = src.Image.ToImageSimpleContentReq(_mediaUrl);
        dest.Icon = new SvgContentReq();
        dest.Icon.SourceFile = _mediaUrl.GetMediaUrl(src.Icon, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x)).ToString();

        dest.Page = new ContentReq(); // TODO Populate rest of the properties
        dest.Page.SchemaAlias = nameof(PlatformsSystemSchema.Sys__offeringPage).ToLower();
        
        dest.FormState = ctx.Map<OfferingContent, DonationFormStateReq>(src);
        
        dest.Options = new OfferingOptionsReq();
        dest.Options.AllowCrowdfunding = src.AllowCrowdfunding;

        if (src.Content().IsPublished()) {
            dest.Activate = true;
        }
    }
}
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Media;
using System;
using System.Linq;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class QurbaniSeasonLocationReqMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;

    public QurbaniSeasonLocationReqMapping(IMediaUrl mediaUrl) {
        _mediaUrl = mediaUrl;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<QurbaniSeasonLocationContent, QurbaniSeasonLocationReq>((_, _) => new QurbaniSeasonLocationReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(QurbaniSeasonLocationContent src, QurbaniSeasonLocationReq dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.Summary = src.Summary;
        dest.SoldOut = src.SoldOut;
        dest.Categories = src.Categories.Select(x => x.Name).ToList();
        dest.Icon = new SvgContentReq();
        dest.Icon.SourceFile = _mediaUrl.GetMediaUrl(src.Icon, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x)).ToString();
        dest.Image = src.Image.ToImageSimpleContentReq(_mediaUrl);
        dest.Description = new RichTextContentReq();
        dest.Description.Html = src.Description.ToHtmlString(); 
        dest.FormState = src.ToDonationFormStateReq();
    }
}
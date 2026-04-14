using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Media;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class QurbaniSeasonGroupReqMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;

    public QurbaniSeasonGroupReqMapping(IMediaUrl mediaUrl) {
        _mediaUrl = mediaUrl;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<QurbaniSeasonGroupContent, QurbaniSeasonGroupReq>((_, _) => new QurbaniSeasonGroupReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(QurbaniSeasonGroupContent src, QurbaniSeasonGroupReq dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.Summary = src.Summary;
        dest.SoldOut = src.SoldOut;
        dest.Categories = src.Categories.Select(x => x.Name).ToList();
        dest.Icon = new SvgContentReq();
        dest.Icon = src.Icon.ToSvgContentReq(_mediaUrl);
        dest.Image = src.Image.ToImageSimpleContentReq(_mediaUrl);
        dest.Description = src.Description.ToHtmlString().ToRichTextContentReq(); 
        dest.FormState = src.ToDonationFormStateReq();
    }
}
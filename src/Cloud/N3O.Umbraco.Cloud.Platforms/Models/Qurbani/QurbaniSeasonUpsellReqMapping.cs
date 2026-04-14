using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Media;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class QurbaniSeasonUpsellReqMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;

    public QurbaniSeasonUpsellReqMapping(IMediaUrl mediaUrl) {
        _mediaUrl = mediaUrl;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<QurbaniSeasonUpsellContent, QurbaniSeasonUpsellReq>((_, _) => new QurbaniSeasonUpsellReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(QurbaniSeasonUpsellContent src, QurbaniSeasonUpsellReq dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.Summary = src.Summary;
        dest.Icon = new SvgContentReq();
        dest.Icon = src.Icon.ToSvgContentReq(_mediaUrl);
        dest.FormState = src.ToDonationFormStateReq();
    }
}
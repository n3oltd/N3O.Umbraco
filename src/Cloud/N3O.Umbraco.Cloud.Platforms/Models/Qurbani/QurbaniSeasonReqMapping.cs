using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class QurbaniSeasonReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<QurbaniSeasonContent, QurbaniSeasonReq>((_, _) => new QurbaniSeasonReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(QurbaniSeasonContent src, QurbaniSeasonReq dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.Activate = true;
        dest.Options = ctx.Map<QurbaniSeasonContent, QurbaniSeasonOptionsReq>(src);

        dest.Categories = src.Categories.Select(ctx.Map<QurbaniSeasonCategoryContent, QurbaniSeasonCategoryReq>).ToList();
        dest.Groups = src.Groups.Select(ctx.Map<QurbaniSeasonGroupContent, QurbaniSeasonGroupReq>).ToList();
        dest.Locations = src.Locations.Select(ctx.Map<QurbaniSeasonLocationContent, QurbaniSeasonLocationReq>).ToList();
        dest.Upsells = src.Upsells.Select(ctx.Map<QurbaniSeasonUpsellContent, QurbaniSeasonUpsellReq>).ToList();
    }
}
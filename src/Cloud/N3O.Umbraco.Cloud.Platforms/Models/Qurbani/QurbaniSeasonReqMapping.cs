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

        dest.Categories = src.Categories.Select(x => ctx.Map<QurbaniSeasonCategoryContent, QurbaniSeasonCategoryReq>(x)).ToList();
        dest.Groups = src.Groups.Select(x => ctx.Map<QurbaniSeasonGroupContent, QurbaniSeasonGroupReq>(x)).ToList();
        dest.Locations = src.Locations.Select(x => ctx.Map<QurbaniSeasonLocationContent, QurbaniSeasonLocationReq>(x)).ToList();
        dest.Upsells = src.Upsells.Select(x => ctx.Map<QurbaniSeasonUpsellContent, QurbaniSeasonUpsellReq>(x)).ToList();
    }
}
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class QurbaniSeasonReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<QurbaniSeasonContent, QurbaniSeasonPlatformsSettingsReq>((_, _) => new QurbaniSeasonPlatformsSettingsReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(QurbaniSeasonContent src, QurbaniSeasonPlatformsSettingsReq dest, MapperContext ctx) {
        dest.Categories = src.Categories.Select(ctx.Map<QurbaniSeasonCategoryContent, QurbaniSeasonCategoryReq>).ToList();
    }
}
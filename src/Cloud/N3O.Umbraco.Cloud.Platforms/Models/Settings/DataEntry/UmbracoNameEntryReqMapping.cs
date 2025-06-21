using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class UmbracoNameEntryReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<NameEntryContent, UmbracoNameEntryReq>((_, _) => new UmbracoNameEntryReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(NameEntryContent src, UmbracoNameEntryReq dest, MapperContext ctx) {
        dest.Layout = src.Layout.ToEnum<NameLayout>();
    }
}
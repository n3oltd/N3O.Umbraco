using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using System;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models.Settings.DataEntry;

public class NameMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<NameEntryContent, UmbracoNameEntryReq>((_, _) => new UmbracoNameEntryReq(), Map);
    }

    private void Map(NameEntryContent src, UmbracoNameEntryReq dest, MapperContext ctx) {
        dest.Layout = (NameLayout) Enum.Parse(typeof(NameLayout), src.Layout.Id, true);
    }
}
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using System;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models.Settings.DataEntry;

public class AddressMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<AddressEntryContent, UmbracoAddressEntryReq>((_, _) => new UmbracoAddressEntryReq(), Map);
    }

    private void Map(AddressEntryContent src, UmbracoAddressEntryReq dest, MapperContext ctx) {
        dest.Layout = (AddressLayout) Enum.Parse(typeof(AddressLayout), src.Layout.Id, true);
        dest.AddressLookupApiKey = src.LookupApiKey;
    }
}
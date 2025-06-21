using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class ConnectPreferenceSelectionReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IConsentChoice, ConnectPreferenceSelectionReq>((_, _) => new ConnectPreferenceSelectionReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IConsentChoice src, ConnectPreferenceSelectionReq dest, MapperContext ctx) {
        dest.Channel = src.Channel.ToEnum<Channel>();
        dest.Category = src.Category.Name;
        dest.Preference = src.Response.Value;
    }
}
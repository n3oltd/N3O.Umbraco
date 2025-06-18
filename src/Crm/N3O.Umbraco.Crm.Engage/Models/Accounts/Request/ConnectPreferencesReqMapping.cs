using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Crm.Engage.Clients;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crm.Models;

public class ConnectPreferencesReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IConsent, ConnectPreferencesReq>((_, _) => new ConnectPreferencesReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IConsent src, ConnectPreferencesReq dest, MapperContext ctx) {
        dest.Selections = src.Choices
                             .OrEmpty()
                             .Select(ctx.Map<IConsentChoice, ConnectPreferenceSelectionReq>)
                             .ToList();
    }
}
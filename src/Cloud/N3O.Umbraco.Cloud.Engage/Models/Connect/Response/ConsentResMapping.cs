using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class ConsentResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConnectPreferencesRes, ConsentRes>((_, _) => new ConsentRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConnectPreferencesRes src, ConsentRes dest, MapperContext ctx) {
        dest.Choices = src.Selections
                          .OrEmpty()
                          .Select(ctx.Map<ConnectPreferenceSelectionRes, ConsentChoiceRes>)
                          .ToList();
    }
}
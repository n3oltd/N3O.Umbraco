using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Accounts.Models;

public class ConsentChoiceMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConsentChoice, ConsentChoiceRes>((_, _) => new ConsentChoiceRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConsentChoice src, ConsentChoiceRes dest, MapperContext ctx) {
        dest.Channel = src.Channel;
        dest.Category = src.Category;
        dest.Response = src.Response;
    }
}

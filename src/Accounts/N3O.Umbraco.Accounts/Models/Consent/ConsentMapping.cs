using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Accounts.Models {
    public class ConsentMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Consent, ConsentRes>((_, _) => new ConsentRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(Consent src, ConsentRes dest, MapperContext ctx) {
            dest.Choices = src.Choices.OrEmpty().Select(ctx.Map<ConsentChoice, ConsentChoiceRes>).ToList();
        }
    }
}
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Donations.Content;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Donations.Models {
    public class DonationFormMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<DonationFormContent, DonationFormRes>((_, _) => new DonationFormRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(DonationFormContent src, DonationFormRes dest, MapperContext ctx) {
            dest.Title = src.Title;
            dest.Options = src.GetOptions()
                              .OrEmpty()
                              .Select(ctx.Map<DonationOptionContent, DonationOptionRes>)
                              .ToList();
        }
    }
}
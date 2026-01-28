using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class DonationFormMapping : IMapDefinition {
    private readonly ILookups _lookups;
    
    public DonationFormMapping(ILookups lookups) {
        _lookups = lookups;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DonationFormContent, DonationFormRes>((_, _) => new DonationFormRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(DonationFormContent src, DonationFormRes dest, MapperContext ctx) {
        dest.Title = src.Title;
        dest.Options = src.GetOptions(_lookups, new VariationContext(LocalizationSettings.CultureCode))
                          .OrEmpty()
                          .Select(ctx.Map<DonationOptionContent, DonationOptionRes>)
                          .ToList();
    }
}

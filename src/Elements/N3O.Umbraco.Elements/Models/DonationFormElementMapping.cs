using N3O.Umbraco.Elements.Content;
using System;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class DonationFormElementMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<GivingContent, DonationFormElement>((_, _) => new DonationFormElement(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(GivingContent src, DonationFormElement dest, MapperContext ctx) {
        dest.Id = src.Id;
        dest.CheckoutProfileId = Guid.Parse(src.CheckoutProfileId);
        dest.RootCategories = src.RootCategories
                                 .Select(ctx.Map<DonationCategoryContent, DonationCategoryPartial>)
                                 .ToList();
    }
}
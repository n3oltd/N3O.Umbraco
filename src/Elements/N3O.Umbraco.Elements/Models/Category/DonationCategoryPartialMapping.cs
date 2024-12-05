using N3O.Umbraco.Elements.Content;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Extensions;

namespace N3O.Umbraco.Elements.Models;

public class DonationCategoryPartialMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DonationCategoryContent, DonationCategoryPartial>((_, _) => new DonationCategoryPartial(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(DonationCategoryContent src, DonationCategoryPartial dest, MapperContext ctx) {
        dest.Id = src.Id;
        dest.ParentId = src.ParentId;
        dest.Type = src.Type.Id;
        dest.Image = src.ImageUrl;
        dest.Dimension = src.Dimension.IfNotNull(ctx.Map<DimensionDonationCategoryContent, DimensionCategoryData>);
        dest.Ephemeral = src.Ephemeral.IfNotNull(ctx.Map<EphemeralDonationCategoryContent, EphemeralCategoryData>);
        dest.General = src.General.IfNotNull(ctx.Map<GeneralDonationCategoryContent, GeneralCategoryData>);
        dest.Entries = GetMappedEntries(src, ctx);
    }

    private IEnumerable<EntryData> GetMappedEntries(DonationCategoryContent src, MapperContext ctx) {
        foreach (var category in src.Categories.OrderBy(x => x.Name)) {
            yield return ctx.Map<DonationCategoryContent, EntryData>(category);
        }
        
        // TODO Swap this out for popularity based sorting
        foreach (var option in src.Options.OrderBy(x => x.Name)) {
            yield return ctx.Map<DonationOptionContent, EntryData>(option);
        }
    }
}
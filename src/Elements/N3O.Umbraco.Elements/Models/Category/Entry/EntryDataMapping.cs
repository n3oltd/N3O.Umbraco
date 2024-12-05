using N3O.Umbraco.Elements.Content;
using N3O.Umbraco.Elements.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class EntryDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DonationCategoryContent, EntryData>((_, _) => new EntryData(), MapCategory);
        mapper.Define<DonationOptionContent, EntryData>((_, _) => new EntryData(), MapOption);
    }
    
    // Umbraco.Code.MapAll
    private void MapCategory(DonationCategoryContent src, EntryData dest, MapperContext ctx) {
        dest.TypeId = EntryTypes.Category.Id;
        dest.Name = src.Name;
        dest.Image = src.ImageUrl;
        dest.Category = ctx.Map<DonationCategoryContent, CategoryEntryData>(src);
        dest.Option = null;
    }
    
    // Umbraco.Code.MapAll
    private void MapOption(DonationOptionContent src, EntryData dest, MapperContext ctx) {
        dest.TypeId = EntryTypes.Option.Id;
        dest.Name = src.Name;
        dest.Image = src.ImageUrl;
        dest.Category = null;
        dest.Option = ctx.Map<DonationOptionContent, OptionEntryData>(src);
    }
}
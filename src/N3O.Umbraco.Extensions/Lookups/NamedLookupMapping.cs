using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Lookups;

public class NamedLookupMapping : IMapDefinition {
    private readonly IStringLocalizer _stringLocalizer;

    public NamedLookupMapping(IStringLocalizer stringLocalizer) {
        _stringLocalizer = stringLocalizer;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<INamedLookup, NamedLookupRes>((_, _) => new NamedLookupRes(), Map);
    }

    // Umbraco.Code.MapAll -Id
    private void Map(INamedLookup src, NamedLookupRes dest, MapperContext ctx) {
        ctx.Map<ILookup, LookupRes>(src, dest);
        
        dest.Name = _stringLocalizer.Get(TextFolders.Lookups, src.GetType().GetFriendlyName(), src.Name);
    }
}

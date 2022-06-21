using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Opayo.Models;

public class MerchantSessionKeyMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IMerchantSessionKey, MerchantSessionKeyRes>((_, _) => new MerchantSessionKeyRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IMerchantSessionKey src, MerchantSessionKeyRes dest, MapperContext ctx) {
        dest.Key = src.Key;
        dest.ExpiresAt = src.ExpiresAt;
    }
}

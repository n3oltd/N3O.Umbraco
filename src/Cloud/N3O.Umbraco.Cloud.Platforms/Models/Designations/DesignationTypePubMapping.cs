using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public abstract class PublishedDesignationTypeMapping<TDesignationType> : IMapDefinition where TDesignationType : new() {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IDesignation, TDesignationType>((_, _) => new TDesignationType(), Map);
    }

    protected abstract void Map(IDesignation src, TDesignationType dest, MapperContext ctx);
    
    protected bool HasPricing(IPublishedContent designation) {
        var price = (IPrice) designation;
        var pricingRules = (IPricingRules) designation;
        
        if (price.PriceAmount > 0) {
            return true;
        }

        foreach (var rule in pricingRules.PriceRules.OrEmpty()) {
            if (rule.PriceAmount > 0) {
                return true;
            }
        }

        return false;
    }
}
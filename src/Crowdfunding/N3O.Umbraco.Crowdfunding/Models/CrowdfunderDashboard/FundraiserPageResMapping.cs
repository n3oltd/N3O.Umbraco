using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserPageResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) { 
        mapper.Define<Crowdfunder, FundraiserPageRes>((_, _) => new FundraiserPageRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(Crowdfunder src, FundraiserPageRes dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.Owner = src.OwnerName;
        dest.Url = src.Url;
        dest.Status = StaticLookups.GetAll<CrowdfunderStatus>().Single(x => x.Key == src.StatusKey);
    }
}
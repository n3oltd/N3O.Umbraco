using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderDashboardPageResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) { 
        mapper.Define<Crowdfunder, CrowdfunderDashboardEntryRes>((_, _) => new CrowdfunderDashboardEntryRes(), Map);
    }

    private void Map(Crowdfunder src, CrowdfunderDashboardEntryRes dest, MapperContext ctx) {
        dest.Name = src.Name;
        dest.OwnerName = src.OwnerName;
        dest.Url = src.Url;
        dest.Status = StaticLookups.GetAll<CrowdfunderStatus>().Single(x => x.Key == src.StatusKey);
    }
}
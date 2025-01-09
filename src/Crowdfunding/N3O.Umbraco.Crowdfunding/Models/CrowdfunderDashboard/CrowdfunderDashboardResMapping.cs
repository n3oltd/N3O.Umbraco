using N3O.Umbraco.Crowdfunding.Entities;
using NPoco;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderDashboardResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) { 
        mapper.Define<Page<Crowdfunder>, CrowdfunderDashboardRes>((_, _) => new CrowdfunderDashboardRes(), Map);
    }

    private void Map(Page<Crowdfunder> src, CrowdfunderDashboardRes dest, MapperContext ctx) {
        dest.Entries = src.Items.Select(ctx.Map<Crowdfunder, CrowdfunderDashboardEntryRes>);
        dest.CurrentPage = src.CurrentPage;
        dest.HasMoreEntries = src.CurrentPage < src.TotalPages;
    }
}
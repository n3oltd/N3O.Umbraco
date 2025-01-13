using N3O.Umbraco.Crowdfunding.Entities;
using NPoco;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserDashboardResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) { 
        mapper.Define<Page<Crowdfunder>, FundraiserDashboardRes>((_, _) => new FundraiserDashboardRes(), Map);
    }

    private void Map(Page<Crowdfunder> src, FundraiserDashboardRes dest, MapperContext ctx) {
        dest.Entries = src.Items.Select(ctx.Map<Crowdfunder, FundraiserDashboardPageRes>);
        dest.CurrentPage = src.CurrentPage;
        dest.HasMoreEntries = src.CurrentPage < src.TotalPages;
    }
}
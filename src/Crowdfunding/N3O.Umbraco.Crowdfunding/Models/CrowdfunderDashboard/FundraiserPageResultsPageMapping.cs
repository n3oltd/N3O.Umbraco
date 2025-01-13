using N3O.Umbraco.Crowdfunding.Entities;
using NPoco;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserPageResultsPageMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) { 
        mapper.Define<Page<Crowdfunder>, FundraiserPageResultsPage>((_, _) => new FundraiserPageResultsPage(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(Page<Crowdfunder> src, FundraiserPageResultsPage dest, MapperContext ctx) {
        dest.Entries = src.Items.Select(ctx.Map<Crowdfunder, FundraiserPageRes>);
        dest.CurrentPage = (int) src.CurrentPage;
        dest.HasMoreEntries = src.CurrentPage < src.TotalPages;
    }
}
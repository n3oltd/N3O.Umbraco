using N3O.Umbraco.Cloud.Platforms.Clients;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedAnalyticsParametersMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IEnumerable<DataListItem>, PublishedAnalyticsParameters>((_, _) => new PublishedAnalyticsParameters(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IEnumerable<DataListItem> src, PublishedAnalyticsParameters dest, MapperContext ctx) {
        dest.Tags = src.ToDictionary(x => x.Name, x => x.Value);
    }
}
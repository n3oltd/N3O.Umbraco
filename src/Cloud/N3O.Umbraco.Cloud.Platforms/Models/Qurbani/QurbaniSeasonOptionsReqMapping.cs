using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class QurbaniSeasonOptionsReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<QurbaniSeasonContent, QurbaniSeasonOptionsReq>((src, context) => {
            var dest = new QurbaniSeasonOptionsReq();
            dest.ShowOnBehalfOf = src.ShowOnBehalfOf;

            return dest;
        });
    }
}
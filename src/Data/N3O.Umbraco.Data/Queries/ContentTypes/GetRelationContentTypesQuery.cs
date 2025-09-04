using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Queries;

public class GetRelationContentTypesQuery : Request<string, IEnumerable<ContentTypeRes>> {
    public GetRelationContentTypesQuery(ContentId contentId) {
        ContentId = contentId;
    }
    
    public ContentId ContentId { get; }
}

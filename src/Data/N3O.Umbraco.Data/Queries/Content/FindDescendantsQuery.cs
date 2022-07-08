using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Criteria;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Queries;

public class FindDescendantsQuery : Request<ContentCriteria, IEnumerable<ContentRes>> {
    public FindDescendantsQuery(ContentId contentId) {
        ContentId = contentId;
    }
    
    public ContentId ContentId { get; }
}
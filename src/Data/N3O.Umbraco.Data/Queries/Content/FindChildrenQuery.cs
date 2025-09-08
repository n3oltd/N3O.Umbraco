using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Criteria;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Queries;

public class FindChildrenQuery : Request<ContentCriteria, IEnumerable<ContentRes>> {
    public FindChildrenQuery(ContentId contentId) {
        ContentId = contentId;
    }
    
    public ContentId ContentId { get; }
}

using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Queries {
    public class GetDescendantsContentTypesQuery : Request<None, IEnumerable<ContentTypeSummary>> {
        public GetDescendantsContentTypesQuery(ContentId contentId) {
            ContentId = contentId;
        }
        
        public ContentId ContentId { get; }
    }
}
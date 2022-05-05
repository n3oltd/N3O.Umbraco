using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Queries {
    public class GetAllowedContentTypesQuery : Request<None, IEnumerable<ContentTypeSummary>> {
        public GetAllowedContentTypesQuery(ContentId contentId) {
            ContentId = contentId;
        }
        
        public ContentId ContentId { get; }
    }
}
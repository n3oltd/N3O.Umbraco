using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Data.Queries {
    public class GetImportTemplateQuery : Request<None, ImportTemplate> {
        public GetImportTemplateQuery(ContentId contentId, ContentType contentType) {
            ContentId = contentId;
            ContentType = contentType;
        }
        
        public ContentId ContentId { get; }
        public ContentType ContentType { get; }
    }
}
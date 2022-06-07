using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Data.Commands {
    public class QueueImportsCommand : Request<QueueImportsReq, QueueImportsRes> {
        public QueueImportsCommand(ContentId contentId, ContentType contentType) {
            ContentId = contentId;
            ContentType = contentType;
        }
        
        public ContentId ContentId { get; }
        public ContentType ContentType { get; }
    }
}
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Data.Queries {
    public class GetContentTypeByAliasQuery : Request<None, ContentTypeRes> {
        public GetContentTypeByAliasQuery(ContentType contentType) {
            ContentType = contentType;
        }

        public ContentType ContentType { get; }
    }
}
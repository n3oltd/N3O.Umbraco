using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Data.Queries;

public class GetImportablePropertiesQuery : Request<None, DataProperties> {
    public GetImportablePropertiesQuery(ContentType contentType) {
        ContentType = contentType;
    }
    
    public ContentType ContentType { get; }
}

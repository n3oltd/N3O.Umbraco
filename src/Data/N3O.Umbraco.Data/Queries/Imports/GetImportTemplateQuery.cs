using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Data.Queries;

public class GetImportTemplateQuery : Request<ImportTemplateReq, ImportTemplate> {
    public GetImportTemplateQuery(ContentType contentType) {
        ContentType = contentType;
    }
    
    public ContentType ContentType { get; }
}

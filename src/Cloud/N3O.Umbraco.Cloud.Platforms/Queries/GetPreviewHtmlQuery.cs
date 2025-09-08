using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Cloud.Platforms.NamedParameters;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Queries;

public class GetPreviewHtmlQuery : Request<Dictionary<string, object>, PreviewRes> {
    public GetPreviewHtmlQuery(ContentTypeAlias contentTypeAlias) {
        ContentTypeAlias = contentTypeAlias;
    }
    
    public ContentTypeAlias ContentTypeAlias { get; }
}
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Queries;

public class GetPreviewHtmlQuery : Request<Dictionary<string, object>, PreviewHtmlRes> {
    public GetPreviewHtmlQuery(ContentId contentId) {
        ContentId = contentId;
    }

    public ContentId ContentId { get; }
}

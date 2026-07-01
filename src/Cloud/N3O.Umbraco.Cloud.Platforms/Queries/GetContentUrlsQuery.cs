using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Cloud.Platforms.Queries;

public class GetContentUrlsQuery : Request<None, ContentUrlsRes> {
    public GetContentUrlsQuery(ContentId contentId) {
        ContentId = contentId;
    }

    public ContentId ContentId { get; }
}

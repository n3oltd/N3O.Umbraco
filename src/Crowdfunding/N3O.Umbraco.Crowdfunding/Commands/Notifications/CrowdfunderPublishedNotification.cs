using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class CrowdfunderPublishedNotification : Request<None, None> {
    public CrowdfunderTypeId TypeId { get; set; }
    public ContentId ContentId { get; set; }
    
    public CrowdfunderPublishedNotification(CrowdfunderTypeId typeId, ContentId contentId) {
        TypeId = typeId;
        ContentId = contentId;
    }
}
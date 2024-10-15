using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class CrowdfunderUpdatedNotification : Request<None, None> {
    public CrowdfunderTypeId TypeId { get; set; }
    public ContentId ContentId { get; set; }
    
    public CrowdfunderUpdatedNotification(CrowdfunderTypeId typeId, ContentId contentId) {
        TypeId = typeId;
        ContentId = contentId;
    }
}
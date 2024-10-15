using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class UpdateCrowdfunderStatisticsCommand : Request<None, None> {
    public ContentId ContentId { get; set; }
    public CrowdfunderTypeId TypeId { get; set; }
    
    public UpdateCrowdfunderStatisticsCommand(ContentId contentId, CrowdfunderTypeId typeId) {
        ContentId = contentId;
        TypeId = typeId;
    }
}
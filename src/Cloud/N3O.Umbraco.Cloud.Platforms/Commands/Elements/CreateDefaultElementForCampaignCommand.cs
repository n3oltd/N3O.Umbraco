using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Cloud.Platforms.Commands;

public class CreateDefaultElementForCampaignCommand : Request<None, None> {
    public CreateDefaultElementForCampaignCommand(ContentId contentId) {
        ContentId = contentId;
    }
    
    public ContentId ContentId { get; }
}
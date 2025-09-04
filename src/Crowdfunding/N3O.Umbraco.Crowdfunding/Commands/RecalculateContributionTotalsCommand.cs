using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class RecalculateContributionTotalsCommand : Request<None, None> {
    public ContentId ContentId { get; set; }
    
    public RecalculateContributionTotalsCommand(ContentId contentId) {
        ContentId = contentId;
    }
}
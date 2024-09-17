using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Events;

public abstract class PledgeEvent : Request<WebhookPledge, None> {
    public ContentId ContentId { get; }
    
    protected PledgeEvent(ContentId contentId) {
        ContentId = contentId;
    }
}
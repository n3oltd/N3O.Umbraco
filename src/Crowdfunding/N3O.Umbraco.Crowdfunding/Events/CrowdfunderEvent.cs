using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Events;

public abstract class CrowdfunderEvent : Request<WebhookCrowdfunderInfo, None> {
    public ContentId ContentId { get; }
    
    protected CrowdfunderEvent(ContentId contentId) {
        ContentId = contentId;
    }
}
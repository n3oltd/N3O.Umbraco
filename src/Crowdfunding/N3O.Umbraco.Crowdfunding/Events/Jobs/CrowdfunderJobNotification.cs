using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Events;

public abstract class CrowdfunderJobNotification : Request<JobResult, None> {
    public ContentId ContentId { get; }
    
    protected CrowdfunderJobNotification(ContentId contentId) {
        ContentId = contentId;
    }
}
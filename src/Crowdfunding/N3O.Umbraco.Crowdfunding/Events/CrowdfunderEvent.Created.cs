using N3O.Umbraco.Crowdfunding.NamedParameters;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderCreatedEvent : CrowdfunderEvent {
    public CrowdfunderCreatedEvent(ContentId contentId) : base(contentId) { }
}
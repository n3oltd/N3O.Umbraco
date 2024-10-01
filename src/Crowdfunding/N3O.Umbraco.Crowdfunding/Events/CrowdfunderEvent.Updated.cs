using N3O.Umbraco.Crowdfunding.NamedParameters;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderUpdatedEvent : CrowdfunderEvent {
    public CrowdfunderUpdatedEvent(ContentId contentId) : base(contentId) { }
}
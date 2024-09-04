using N3O.Umbraco.Crowdfunding.NamedParameters;

namespace N3O.Umbraco.Crowdfunding.Events;

public class PledgeUpdatedEvent : PledgeEvent {
    public PledgeUpdatedEvent(ContentId contentId) : base(contentId) { }
}
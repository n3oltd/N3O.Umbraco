using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Crowdfunding.Events;

public class PledgeUpdatedEvent : PledgeEvent {
    public PledgeUpdatedEvent(ContentId contentId) : base(contentId) { }
}
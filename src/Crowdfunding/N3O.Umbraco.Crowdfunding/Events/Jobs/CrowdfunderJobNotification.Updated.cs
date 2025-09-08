using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderUpdatedJobNotification : CrowdfunderJobNotification {
    public CrowdfunderUpdatedJobNotification(ContentId contentId) : base(contentId) { }
}
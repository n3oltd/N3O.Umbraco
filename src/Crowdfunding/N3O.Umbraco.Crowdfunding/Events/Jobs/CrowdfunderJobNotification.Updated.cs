using N3O.Umbraco.Crowdfunding.NamedParameters;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderUpdatedJobNotification : CrowdfunderJobNotification {
    public CrowdfunderUpdatedJobNotification(ContentId contentId) : base(contentId) { }
}
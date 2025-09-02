using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderCreatedJobNotification : CrowdfunderJobNotification {
    public CrowdfunderCreatedJobNotification(ContentId contentId) : base(contentId) { }
}
using N3O.Umbraco.Crowdfunding.NamedParameters;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderCreatedJobNotification : CrowdfunderJobNotification {
    public CrowdfunderCreatedJobNotification(ContentId contentId) : base(contentId) { }
}
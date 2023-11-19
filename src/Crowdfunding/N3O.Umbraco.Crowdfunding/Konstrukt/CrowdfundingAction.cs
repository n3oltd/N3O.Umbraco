using Konstrukt.Configuration.Actions;

namespace N3O.Umbraco.CrowdFunding.Konstrukt; 

public class CrowdfundingAction : KonstruktAction<KonstruktActionResult> {
    public override string Alias { get; }
    public override string Name { get; }

    public override bool IsVisible(KonstruktActionVisibilityContext ctx) {
        return false;
    }

    public override KonstruktActionResult Execute(string collectionAlias, object[] entityIds) {
        throw new System.NotImplementedException();
    }
}
using N3O.Umbraco.Entities;
using NodaTime;

namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    public override void OnSaving(Instant timestamp, RevisionBehaviour revisionBehaviour) {
        base.OnSaving(timestamp, revisionBehaviour);

        if (Progress.CurrentStage.IsComplete(this)) {
            Progress = Progress.NextStage();
        }
    }
}

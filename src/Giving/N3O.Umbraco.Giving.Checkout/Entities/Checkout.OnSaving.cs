using NodaTime;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public override void OnSaving(Instant timestamp) {
            base.OnSaving(timestamp);

            if (Progress.CurrentStage.IsComplete(this)) {
                Progress = Progress.NextStage();
            }
        }
    }
}